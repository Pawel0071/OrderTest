using System.Threading.Tasks;

namespace OrderTest.Interfaces;

/// <summary>
/// Definiuje operacje na repozytorium zamówień.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Inicjalizuje repozytorium zamówień w pamięci, dodając przykładowe dane.
    /// </summary>
    /// <returns>True, jeśli inicjalizacja zakończyła się sukcesem.</returns>
    Task<bool> InitOrdersAsync();

    /// <summary>
    /// Dodaje nowe zamówienie na podstawie opisu.
    /// </summary>
    /// <param name="description">Opis zamówienia.</param>
    /// <returns>Identyfikator dodanego zamówienia.</returns>
    /// <exception cref="ArgumentException">Rzucany, gdy opis jest niepoprawny.</exception>
    Task<int> AddOrderAsync(string description);

    /// <summary>
    /// Pobiera szczegóły zamówienia na podstawie jego identyfikatora.
    /// </summary>
    /// <param name="orderId">Identyfikator zamówienia.</param>
    /// <returns>Opis zamówienia w formacie tekstowym.</returns>
    /// <exception cref="ArgumentException">Rzucany, gdy identyfikator jest niepoprawny.</exception>
    /// <exception cref="KeyNotFoundException">Rzucany, gdy zamówienie nie istnieje.</exception>
    Task<string> GetOrderAsync(int orderId);

    /// <summary>
    /// Usuwa zamówienie o podanym identyfikatorze.
    /// </summary>
    /// <param name="orderId">Identyfikator zamówienia do usunięcia.</param>
    /// <returns>True, jeśli zamówienie zostało usunięte.</returns>
    /// <exception cref="ArgumentException">Rzucany, gdy identyfikator jest niepoprawny.</exception>
    /// <exception cref="KeyNotFoundException">Rzucany, gdy zamówienie nie istnieje.</exception>
    Task<bool> DeleteOrderAsync(int orderId);

    /// <summary>
    /// Aktualizuje opis istniejącego zamówienia.
    /// </summary>
    /// <param name="orderId">Identyfikator zamówienia.</param>
    /// <param name="description">Nowy opis zamówienia.</param>
    /// <returns>True, jeśli aktualizacja zakończyła się sukcesem.</returns>
    /// <exception cref="ArgumentException">Rzucany, gdy dane są niepoprawne.</exception>
    /// <exception cref="KeyNotFoundException">Rzucany, gdy zamówienie nie istnieje.</exception>
    Task<bool> UpdateOrderAsync(int orderId, string description);
}
