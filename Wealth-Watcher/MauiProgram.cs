using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Wealth_Watcher.Services;
using Wealth_Watcher.Services.UserServiceImpl.cs;

namespace Wealth_Watcher
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddMudServices();

            builder.Services.AddMauiBlazorWebView();
            var userDataPath = Environment.GetEnvironmentVariable("MYAPP_USER_DATA");
            if (string.IsNullOrEmpty(userDataPath))
            {
                throw new InvalidOperationException("Environment variable for user data path is not set.");
            }
            var userFilePath = Path.Combine(userDataPath, "users.json");
            builder.Services.AddSingleton<IUserService>(new UserService(userFilePath));


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
