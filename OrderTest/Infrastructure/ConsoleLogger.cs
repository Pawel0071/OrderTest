using OrderTest.Interfaces;

namespace OrderTest.Infrastructure;

using System;

public class ConsoleLogger(string level) : ILogger
{
    public void LogInfo(string message)
    {
        if (IsEnabled("Information"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[INFO] {DateTime.Now}: {message}");
            Console.ResetColor();
        }
    }

    public void LogError(string message, Exception ex = null)
    {
        if (IsEnabled("Error"))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {DateTime.Now}: {message}");
            if (ex != null)
            {
                Console.WriteLine($"Exception: {ex.GetType().Name} - {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

            Console.ResetColor();
        }
    }
    
    private bool IsEnabled(string level1)
    {
        var levels = new Dictionary<string, int>
        {
            ["Trace"] = 0,
            ["Debug"] = 1,
            ["Information"] = 2,
            ["Warning"] = 3,
            ["Error"] = 4,
            ["Critical"] = 5,
            ["None"] = 6
        };

        return levels[level1] >= levels[level];
    }
}