using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Wealth_Watcher.Services;
using Wealth_Watcher.Services.UserServiceImpl.cs;
using Wealth_Watcher.Services.NewFolder;

namespace Wealth_Watcher
{
    public static class MauiProgram
    {
        // Initializes and configures the MAUI app
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>() // Sets up the main app configuration
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); // Adds custom fonts
                });
            builder.Services.AddMudServices(); // Adds MudBlazor services
            builder.Services.AddSingleton<UserSessionService>(); // Registers user session services as a singleton
            builder.Services.AddMauiBlazorWebView(); // Adds Blazor WebView to the MAUI app

            // Retrieves and verifies the user data path environment variable
            var userDataPath = Environment.GetEnvironmentVariable("MYAPP_USER_DATA");
            if (string.IsNullOrEmpty(userDataPath))
            {
                throw new InvalidOperationException("Environment variable for user data path is not set.");
            }

            var userFilePath = Path.Combine(userDataPath, "users.json"); // Defines the path for user data
            // Configures and registers the user service
            builder.Services.AddSingleton<IUserService>((services) =>
            {
                var userSessionService = services.GetRequiredService<UserSessionService>();
                return new UserService(userFilePath, userSessionService);
            });

            var debtFilePath = Path.Combine(userDataPath, "debts.json");// Sets the path for debt data
            builder.Services.AddSingleton<IDebtService>((services) =>
            {
                // Configures and registers the debt service
                var userSessionService = services.GetRequiredService<UserSessionService>();
                return new DebtService(debtFilePath, userSessionService);
            });
    

            var transactionFilePath = Path.Combine(userDataPath, "transactions.json"); // Sets the path for transaction data
            // Configures and registers the transaction service
            builder.Services.AddSingleton<ITransactionService>((services) =>
            {
                var userSessionService = services.GetRequiredService<UserSessionService>();
                return new TransactionService(userSessionService, transactionFilePath);
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools(); // Adds developer tools for debugging
            builder.Logging.AddDebug(); // Adds logging for debug builds
#endif

            return builder.Build(); // Builds and returns the configured MauiApp
        }
    }
}
