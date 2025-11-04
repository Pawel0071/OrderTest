namespace OrderTest.Interfaces
{
    /// <summary>
    /// Definiuje usługę powiadamiania o zakończeniu operacji na zamówieniach.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Wysyła powiadomienie o zakończeniu przetwarzania zamówienia.
        /// </summary>
        /// <param name="orderId">Identyfikator przetworzonego zamówienia.</param>
        /// <param name="message">Treść powiadomienia.</param>
        void Send(int orderId, string message);
    }
}