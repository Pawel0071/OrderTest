using OrderTest.Interfaces;

namespace OrderTest.Infrastructure;

public class OrderValidator : IOrderValidator
{
    public bool IsValidId(int orderId)
    {
        return orderId > 0;
    }

    public bool IsValidDescription(string? description)
    {
        return !string.IsNullOrWhiteSpace(description) && description.Length <= 200;
    }
}
