namespace OrderTest.Interfaces;

public interface IOrderService
{
    Task ProcessOrderAsync(int orderId);
    
    Task InitInMemoryRepositoryAsync();

    Task<int> AddOrderAsync(string description);
}
