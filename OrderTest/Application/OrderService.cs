using OrderTest.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace OrderTest.Application;

public class OrderService(IOrderRepository repository, ILogger logger, IOrderValidator validator,  INotificationService notificationService)
    : IOrderService
{
    private readonly IOrderValidator _validator = validator;

    private static readonly ConcurrentDictionary<int, SemaphoreSlim> OrderLocks = new();
    private static readonly SemaphoreSlim AddOrderLock = new(1, 1);

    public async Task ProcessOrderAsync(int orderId)
    {
        var semaphore = OrderLocks.GetOrAdd(orderId, _ => new SemaphoreSlim(1, 1));
        logger.LogInfo($"Attempting to process order {orderId}");

        await semaphore.WaitAsync();
        bool success = false;

        try
        {
            logger.LogInfo($"Processing order {orderId} on thread {Task.CurrentId}");

            string orderDetails = await repository.GetOrderAsync(orderId);

            if (string.IsNullOrEmpty(orderDetails))
            {
                logger.LogError($"Order {orderId} not found or empty.");
                return;
            }

            logger.LogInfo($"Order retrieved: {orderDetails}");
            success = true;
        }
        catch (ArgumentException argEx)
        {
            logger.LogError($"Validation error while processing order {orderId}.", argEx);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError($"Order {orderId} not found in repository.", knfEx);
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error while processing order {orderId}.", ex);
        }
        finally
        {
            semaphore.Release();
            OrderLocks.TryRemove(orderId, out _);
            logger.LogInfo($"Finished processing order {orderId}");

            if (success)
            {
                notificationService.Send(orderId, "Order processed successfully.");
            }
        }
    }

    public async Task InitInMemoryRepositoryAsync()
    {
        try
        {
            logger.LogInfo("Initializing in-memory order repository...");
            await repository.InitOrdersAsync();
            logger.LogInfo("Repository initialized.");
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to initialize repository.", ex);
        }
    }

    public async Task<int> AddOrderAsync(string description)
    {
        await AddOrderLock.WaitAsync();
        try
        {
            int orderId = await repository.AddOrderAsync(description);
            logger.LogInfo($"Order successfully added with ID: {orderId}");
            notificationService.Send(orderId, "Order added successfully.");
            return orderId;
        }
        catch (ArgumentException argEx)
        {
            logger.LogError("Validation error while adding order.", argEx);
            return -1;
        }
        catch (Exception ex)
        {
            logger.LogError("Unexpected error while adding order.", ex);
            return -1;
        }
        finally
        {
            AddOrderLock.Release();
        }
    }
}