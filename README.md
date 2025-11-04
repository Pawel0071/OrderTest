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
    
    dotnet run
    
1. Sklonuj repozytorium:

```bash
git clone https://github.com/twoje-konto/OrderTest.git
cd OrderTest
```
