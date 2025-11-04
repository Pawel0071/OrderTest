using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderTest.Domain;
using OrderTest.Interfaces;

namespace OrderTest.Repositories
{
    public class OrderRepository(ILogger logger, IOrderValidator validator) : IOrderRepository
    {
        private readonly Dictionary<int, Order> _orders = new();
        private int _nextId = 1;

        public async Task<bool> InitOrdersAsync()
        {
            _orders.Clear();

            await AddOrderAsync("Laptop");
            await AddOrderAsync("Smartphone");
            await AddOrderAsync("Monitor");
            await AddOrderAsync("Klawiatura");

            logger.LogInfo("Initial orders loaded into memory.");
            return true;
        }

        public async Task<int> AddOrderAsync(string description)
        {
            if (!validator.IsValidDescription(description))
            {
                var argEx = new ArgumentException("Invalid order description.", nameof(description));
                logger.LogError("Order description validation failed.", argEx);
                throw argEx;
            }

            var order = new Order
            {
                Id = _nextId++,
                Description = description
            };

            _orders[order.Id] = order;
            logger.LogInfo($"Order added: {order.Id} - {order.Description}");
            return order.Id;
        }

        public async Task<string> GetOrderAsync(int orderId)
        {
            if (!validator.IsValidId(orderId))
            {
                var argEx = new ArgumentException($"Invalid order ID: {orderId}", nameof(orderId));
                logger.LogError("Order ID validation failed.", argEx);
                throw argEx;
            }

            if (_orders.TryGetValue(orderId, out var order))
            {
                return $"Order #{order.Id}: {order.Description}";
            }

            var notFoundEx = new KeyNotFoundException($"Order with ID {orderId} was not found.");
            logger.LogError("Order retrieval failed.", notFoundEx);
            throw notFoundEx;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            if (!validator.IsValidId(orderId))
            {
                var argEx = new ArgumentException($"Invalid order ID: {orderId}", nameof(orderId));
                logger.LogError("Order ID validation failed for deletion.", argEx);
                throw argEx;
            }

            if (_orders.Remove(orderId))
            {
                logger.LogInfo($"Order {orderId} deleted.");
                return true;
            }

            var notFoundEx = new KeyNotFoundException($"Order with ID {orderId} not found for deletion.");
            logger.LogError("Order deletion failed.", notFoundEx);
            throw notFoundEx;
        }

        public async Task<bool> UpdateOrderAsync(int orderId, string description)
        {
            if (!validator.IsValidId(orderId))
            {
                var argEx = new ArgumentException($"Invalid order ID: {orderId}", nameof(orderId));
                logger.LogError("Order ID validation failed for update.", argEx);
                throw argEx;
            }

            if (!validator.IsValidDescription(description))
            {
                var argEx = new ArgumentException("Invalid order description.", nameof(description));
                logger.LogError("Order description validation failed for update.", argEx);
                throw argEx;
            }

            if (_orders.TryGetValue(orderId, out var order))
            {
                order.Description = description;
                logger.LogInfo($"Order {orderId} updated to: {description}");
                return true;
            }

            var notFoundEx = new KeyNotFoundException($"Order with ID {orderId} not found for update.");
            logger.LogError("Order update failed.", notFoundEx);
            throw notFoundEx;
        }
    }
}