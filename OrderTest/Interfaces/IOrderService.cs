using System.Threading.Tasks;

namespace OrderTest.Interfaces;

/// <summary>
/// Definiuje operacje biznesowe związane z obsługą zamówień.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Przetwarza zamówienie o podanym identyfikatorze.
    /// Operacja jest synchronizowana, aby uniknąć równoległego przetwarzania tego samego zamówienia.
    /// </summary>
    /// <param name="orderId">Identyfikator zamówienia do przetworzenia.</param>
    /// <returns>Task reprezentujący operację asynchroniczną.</returns>
    Task ProcessOrderAsync(int orderId);

    /// <summary>
    /// Inicjalizuje repozytorium zamówień w pamięci, wczytując przykładowe dane.
    /// </summary>
    /// <returns>Task reprezentujący operację asynchroniczną.</returns>
    Task InitInMemoryRepositoryAsync();

    /// <summary>
    /// Dodaje nowe zamówienie na podstawie opisu.
    /// W przypadku błędu walidacji lub wyjątku zwraca -1.
    /// </summary>
    /// <param name="description">Opis zamówienia.</param>
    /// <returns>Identyfikator dodanego zamówienia lub -1 w przypadku błędu.</returns>
    Task<int> AddOrderAsync(string description);
}
