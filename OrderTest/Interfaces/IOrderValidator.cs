namespace OrderTest.Interfaces;

    public interface IOrderValidator
    {
        bool IsValidId(int orderId);
        bool IsValidDescription(string? description);
    }