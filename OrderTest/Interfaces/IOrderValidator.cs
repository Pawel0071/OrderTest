namespace OrderTest.Interfaces
{
    /// <summary>
    /// Definiuje metody walidacji danych zamówienia.
    /// </summary>
    public interface IOrderValidator
    {
        /// <summary>
        /// Sprawdza, czy identyfikator zamówienia jest poprawny.
        /// </summary>
        /// <param name="orderId">Identyfikator zamówienia do sprawdzenia.</param>
        /// <returns>True, jeśli identyfikator jest większy niż 0; w przeciwnym razie false.</returns>
        bool IsValidId(int orderId);

        /// <summary>
        /// Sprawdza, czy opis zamówienia jest poprawny.
        /// </summary>
        /// <param name="description">Opis zamówienia do sprawdzenia.</param>
        /// <returns>True, jeśli opis nie jest pusty i ma maksymalnie 200 znaków; w przeciwnym razie false.</returns>
        bool IsValidDescription(string? description);
    }
}