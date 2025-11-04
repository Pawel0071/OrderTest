using OrderTest.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace OrderTest.Application;

public class OrderService(IOrderRepository repository, ILogger logger, IOrderValidator validator)
    : IOrderService
{
    private readonly IOrderValidator _validator = validator;

    private static readonly ConcurrentDictionary<int, SemaphoreSlim> OrderLocks = new();

    public async Task ProcessOrderAsync(int orderId)
    {
        var semaphore = OrderLocks.GetOrAdd(orderId, _ => new SemaphoreSlim(1, 1));

        logger.LogInfo($"Attempting to process order {orderId}");

        await semaphore.WaitAsync();
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
        try
        {
            int orderId = await repository.AddOrderAsync(description);
            logger.LogInfo($"Order successfully added with ID: {orderId}");
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
    }
}