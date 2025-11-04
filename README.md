# 🧾 OrderTest

OrderTest to aplikacja konsolowa w .NET 8.0 służąca do zarządzania zamówieniami w pamięci. Projekt demonstruje architekturę warstwową, walidację danych, obsługę wyjątków, logowanie oraz wykorzystanie Dependency Injection.

## 🧱 Struktura projektu
    
    OrderTest/
    ├── Domain/              # Model domenowy: Order.cs
    ├── Application/         # Logika biznesowa: OrderService, IOrderService
    ├── Infrastructure/      # Logger, walidator
    ├── Repositories/        # OrderRepository z obsługą wyjątków
    ├── Configuration/       # AppConfig do konfiguracji logowania
    ├── Program.cs           # Punkt wejścia aplikacji
    ├── appsettings.json     # Konfiguracja poziomu logowania
    
## ⚙️ Wymagania

  - [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download)
  - Visual Studio / VS Code / Rider
  - System operacyjny: Windows, macOS, Linux

## 🚀 Uruchomienie
    
    
1. Sklonuj repozytorium:

```bash
git clone https://github.com/twoje-konto/OrderTest.git
cd OrderTest
```
2. Upewnij się, że plik appsettings.json istnieje i ma ustawione kopiowanie do katalogu wyjściowego:

```
{
  "Logging": {
    "LogLevel": "Information"
  }
}
```

3. Uruchom aplikację:

```
dotnet run
```

## 🛠️ Konfiguracja logowania

Poziom logowania jest definiowany w appsettings.json:

```
{
  "Logging": {
    "LogLevel": "Information" // Możliwe: Trace, Debug, Information, Warning, Error, Critical, None
  }
}
```

✅ Funkcje

  - Dodawanie, aktualizacja, usuwanie i przetwarzanie zamówień
  - Walidacja danych przez IOrderValidator
  - Obsługa wyjątków: ArgumentException, KeyNotFoundException
  - Synchronizacja dostępu do zamówień przez SemaphoreSlim
  - Konfigurowalny poziom logowania
  - Architektura zgodna z zasadami Clean Architecture

## ISSUES:
- [Unit Test Error} (https://github.com/Pawel0071/OrderTest/issues/1)
