using Microsoft.Extensions.Logging;

namespace TurfApp
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

			builder.Services.AddSingleton<TurfApp.MVVM.View.HomePage>();
			builder.Services.AddSingleton(() => new NavigationPage(new TurfApp.MVVM.View.HomePage()));

#if DEBUG
			builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
