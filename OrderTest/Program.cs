using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderTest.Application;
using OrderTest.Configuration;
using OrderTest.Infrastructure;
using OrderTest.Interfaces;
using OrderTest.Repositories;

namespace OrderTest;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Order Processing System");

        // 📦 Wczytanie konfiguracji z appsettings.json
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var appConfig = configuration.Get<AppConfig>();
        
        // 🔧 Konfiguracja kontenera DI
        var services = new ServiceCollection();

        services.AddSingleton<ILogger>(sp => new ConsoleLogger(appConfig?.Logging.LogLevel ?? "Trace"));
        services.AddSingleton<IOrderValidator, OrderValidator>();
        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddSingleton<IOrderService, OrderService>();
        services.AddSingleton<INotificationService, ConsoleNotificationService>();

        var serviceProvider = services.BuildServiceProvider();

        // 🔄 Pobranie serwisu
        var orderService = serviceProvider.GetRequiredService<IOrderService>();

        // 🧪 Inicjalizacja repozytorium
        await orderService.InitInMemoryRepositoryAsync();

        // 🧵 Przetwarzanie zamówień
        var processingTasks = new[]
        {
            orderService.ProcessOrderAsync(1),
            orderService.ProcessOrderAsync(2),
            orderService.ProcessOrderAsync(-1), // niepoprawne ID
            orderService.ProcessOrderAsync(99)  // nieistniejące ID
        };

        await Task.WhenAll(processingTasks);

        // ➕ Dodawanie nowych zamówień i przetwarzanie
        var addAndProcessTasks = new[]
        {
            orderService.AddOrderAsync("Słuchawki"),
            orderService.AddOrderAsync(string.Empty),
            orderService.ProcessOrderAsync(5)
        };

        await Task.WhenAll(addAndProcessTasks);

        Console.WriteLine("Processing complete.");
    }
}