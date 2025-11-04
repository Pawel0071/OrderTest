using FluentAssertions;
using Moq;
using OrderTest.Application;
using OrderTest.Interfaces;

namespace OrderTest.UnitTests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repoMock = new();
    private readonly Mock<ILogger> _loggerMock = new();
    private readonly Mock<IOrderValidator> _validatorMock = new();
    private readonly Mock<INotificationService> _notificationMock = new();

    public OrderServiceTests()
    {
        _validatorMock.Setup(v => v.IsValidDescription(It.IsAny<string>())).Returns<string>(desc => !string.IsNullOrWhiteSpace(desc));
        _validatorMock.Setup(v => v.IsValidId(It.IsAny<int>())).Returns<int>(id => id > 0);
    }

    [Fact]
    public async Task AddOrderAsync_ShouldReturnId_WhenValid()
    {
        _repoMock.Setup(r => r.AddOrderAsync("Laptop")).ReturnsAsync(1);

        var service = new OrderService(_repoMock.Object, _loggerMock.Object, _validatorMock.Object, _notificationMock.Object);
        int id = await service.AddOrderAsync("Laptop");

        id.Should().Be(1);
        _loggerMock.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("Order successfully added"))), Times.Once);
        _notificationMock.Verify(n => n.Send(1, "Order added successfully."), Times.Once);
    }

    [Fact]
    public async Task AddOrderAsync_ShouldReturnMinusOne_WhenInvalid()
    {
        _validatorMock.Setup(v => v.IsValidDescription("")).Returns(true);
        _repoMock.Setup(r => r.AddOrderAsync("")).ThrowsAsync(new ArgumentException("Invalid order description."));

        var service = new OrderService(_repoMock.Object, _loggerMock.Object, _validatorMock.Object, _notificationMock.Object);
        int id = await service.AddOrderAsync("");

        id.Should().Be(-1);
        _loggerMock.Verify(l => l.LogError(It.Is<string>(s => s.Contains("Validation error while adding order")), It.IsAny<ArgumentException>()), Times.Once);
        _notificationMock.Verify(n => n.Send(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ProcessOrderAsync_ShouldLogAndNotify_WhenOrderExists()
    {
        _repoMock.Setup(r => r.GetOrderAsync(1)).ReturnsAsync("Order #1: Monitor");

        var service = new OrderService(_repoMock.Object, _loggerMock.Object, _validatorMock.Object, _notificationMock.Object);
        await service.ProcessOrderAsync(1);

        _loggerMock.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("Order retrieved"))), Times.Once);
        _notificationMock.Verify(n => n.Send(1, "Order processed successfully."), Times.Once);
    }

    [Fact]
    public async Task ProcessOrderAsync_ShouldLogError_WhenOrderNotFound()
    {
        _repoMock.Setup(r => r.GetOrderAsync(999)).ThrowsAsync(new KeyNotFoundException("Order not found"));

        var service = new OrderService(_repoMock.Object, _loggerMock.Object, _validatorMock.Object, _notificationMock.Object);
        await service.ProcessOrderAsync(999);

        _loggerMock.Verify(l => l.LogError(It.Is<string>(s => s.Contains("not found in repository")), It.IsAny<Exception>()), Times.Once);
        _notificationMock.Verify(n => n.Send(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task InitInMemoryRepositoryAsync_ShouldLogInitialization()
    {
        _repoMock.Setup(r => r.InitOrdersAsync()).ReturnsAsync(true);

        var service = new OrderService(_repoMock.Object, _loggerMock.Object, _validatorMock.Object, _notificationMock.Object);
        await service.InitInMemoryRepositoryAsync();

        _loggerMock.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("Repository initialized"))), Times.Once);
    }
}
