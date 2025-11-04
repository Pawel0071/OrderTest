using System;
using OrderTest.Interfaces;

namespace OrderTest.Infrastructure
{
    public class ConsoleNotificationService : INotificationService
    {
        public void Send(int orderId, string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[NOTIFY] Order {orderId}: {message}");
            Console.ResetColor();
        }
    }
}