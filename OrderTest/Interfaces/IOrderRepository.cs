using OrderTest.Domain;

namespace OrderTest.Interfaces;

public interface IOrderRepository
{
    Task<string> GetOrderAsync(int orderId);
    Task<int> AddOrderAsync(string description);
    
    Task<bool> DeleteOrderAsync(int orderId);
    
    Task<bool> UpdateOrderAsync(int orderId, string description);
    
    Task<bool> InitOrdersAsync();
}