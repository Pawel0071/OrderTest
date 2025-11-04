using System;

namespace OrderTest.Interfaces;

/// <summary>
/// Definiuje metody do logowania informacji i błędów w aplikacji.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Loguje komunikat informacyjny, jeśli poziom logowania na to pozwala.
    /// </summary>
    /// <param name="message">Treść komunikatu do zalogowania.</param>
    void LogInfo(string message);

    /// <summary>
    /// Loguje komunikat błędu wraz z opcjonalnym wyjątkiem, jeśli poziom logowania na to pozwala.
    /// </summary>
    /// <param name="message">Treść komunikatu błędu.</param>
    /// <param name="ex">Opcjonalny wyjątek powiązany z błędem.</param>
    void LogError(string message, Exception? ex = null);
}
