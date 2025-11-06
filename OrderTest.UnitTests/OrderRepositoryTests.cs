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
        // Arrange
        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);

        // Act
        int id = await repo.AddOrderAsync("Laptop");

        // Assert
        id.Should().BeGreaterThan(0);
        _loggerMock.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("Order added"))), Times.Once);
    }

    [Fact]
    public async Task AddOrderAsync_ShouldThrow_WhenDescriptionIsInvalid()
    {
        // Arrange
        _validatorMock.Setup(v => v.IsValidDescription("")).Returns(false);
        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);

        // Act
        Func<Task> act = async () => await repo.AddOrderAsync("");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Invalid order description*");
        _loggerMock.Verify(l => l.LogError(It.Is<string>(s => s.Contains("validation failed")), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task GetOrderAsync_ShouldReturnOrder_WhenExists()
    {
        // Arrange
        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);
        int id = await repo.AddOrderAsync("Monitor");

        // Act
        var result = await repo.GetOrderAsync(id);

        // Assert
        result.Should().Contain("Monitor");
    }

    [Fact]
    public async Task GetOrderAsync_ShouldThrow_WhenOrderNotFound()
    {
        // Arrange
        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);

        // Act
        Func<Task> act = async () => await repo.GetOrderAsync(999);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*was not found*");
        _loggerMock.Verify(l => l.LogError(It.Is<string>(s => s.Contains("retrieval failed")), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task DeleteOrderAsync_ShouldDeleteOrder_WhenExists()
    {
        // Arrange
        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);
        int id = await repo.AddOrderAsync("Smartphone");

        // Act
        var result = await repo.DeleteOrderAsync(id);

        // Assert
        result.Should().BeTrue();
        _loggerMock.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("deleted"))), Times.Once);
    }

    [Fact]
    public async Task UpdateOrderAsync_ShouldUpdateOrder_WhenValid()
    {
        // Arrange
        var repo = new OrderRepository(_loggerMock.Object, _validatorMock.Object);
        int id = await repo.AddOrderAsync("Old");

        // Act
        var result = await repo.UpdateOrderAsync(id, "New");
        var updated = await repo.GetOrderAsync(id);

        // Assert
        result.Should().BeTrue();
        updated.Should().Contain("New");
    }
}