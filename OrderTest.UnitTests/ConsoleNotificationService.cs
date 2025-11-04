using FluentAssertions;
using OrderTest.Infrastructure;
using OrderTest.Interfaces;
using System;
using System.IO;
using Xunit;

namespace OrderTest.UnitTests;

public class ConsoleNotificationServiceTests
{
    private static readonly object ConsoleLock = new();

    [Fact]
    public void Send_ShouldWriteFormattedMessageToConsole()
    {
        lock (ConsoleLock)
        {
            // Arrange
            var service = new ConsoleNotificationService();
            var orderId = 42;
            var message = "Order completed successfully.";

            using var sw = new StringWriter();
            var originalOut = Console.Out;
            Console.SetOut(sw);

            // Act
            service.Send(orderId, message);

            // Restore
            Console.SetOut(originalOut);

            // Assert
            var output = sw.ToString();
            output.Should().Contain($"[NOTIFY] Order {orderId}: {message}");
        }
    }
}
