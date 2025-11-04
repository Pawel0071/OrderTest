using FluentAssertions;
using Moq;
using OrderTest.Interfaces;
using OrderTest.Repositories;

namespace OrderTest.UnitTests;

public class OrderRepositoryTests
{
    private readonly Mock<ILogger> _loggerMock = new();
    private readonly Mock<IOrderValidator> _validatorMock = new();

    public OrderRepositoryTests()
    {
        _validatorMock.Setup(v => v.IsValidId(It.IsAny<int>())).Returns<int>(id => id > 0);
        _validatorMock.Setup(v => v.IsValidDescription(It.IsAny<string>()))
                      .Returns<string>(desc => !string.IsNullOrWhiteSpace(desc));
    }

    [Fact]
    public async Task AddOrderAsync_ShouldAddValidOrder()
    {
        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);
        int id = await repo.AddOrderAsync("Laptop");

        id.Should().BeGreaterThan(0);
        _loggerMock.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("Order added"))), Times.Once);
    }

    [Fact]
    public async Task AddOrderAsync_ShouldThrow_WhenDescriptionIsInvalid()
    {
        _validatorMock.Setup(v => v.IsValidDescription("")).Returns(false);

        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);
        Func<Task> act = async () => await repo.AddOrderAsync("");

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Invalid order description*");

        _loggerMock.Verify(l => l.LogError(It.Is<string>(s => s.Contains("validation failed")), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task GetOrderAsync_ShouldReturnOrder_WhenExists()
    {
        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);
        int id = await repo.AddOrderAsync("Monitor");

        var result = await repo.GetOrderAsync(id);

        result.Should().Contain("Monitor");
    }

    [Fact]
    public async Task GetOrderAsync_ShouldThrow_WhenOrderNotFound()
    {
        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);
        Func<Task> act = async () => await repo.GetOrderAsync(999);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*was not found*");

        _loggerMock.Verify(l => l.LogError(It.Is<string>(s => s.Contains("retrieval failed")), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task DeleteOrderAsync_ShouldDeleteOrder_WhenExists()
    {
        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);
        int id = await repo.AddOrderAsync("Smartphone");

        var result = await repo.DeleteOrderAsync(id);

        result.Should().BeTrue();
        _loggerMock.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("deleted"))), Times.Once);
    }

    [Fact]
    public async Task UpdateOrderAsync_ShouldUpdateOrder_WhenValid()
    {
        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);
        int id = await repo.AddOrderAsync("Old");

        var result = await repo.UpdateOrderAsync(id, "New");

        result.Should().BeTrue();
        var updated = await repo.GetOrderAsync(id);
        updated.Should().Contain("New");
    }
}