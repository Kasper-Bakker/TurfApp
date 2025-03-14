using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.ApplicationModel;
using TurfApp.MVVM.View;

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

			builder.Services.AddSingleton<HomePage>();
			builder.Services.AddSingleton<NavigationPage>(sp => new NavigationPage(sp.GetRequiredService<HomePage>()));

#if ANDROID
			builder.ConfigureLifecycleEvents(events =>
			{
				events.AddAndroid(android => android.OnCreate((activity, bundle) =>
				{
					RequestCameraPermission();
				}));
			});
#endif

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}

		private static async void RequestCameraPermission()
		{
			var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

			if (status != PermissionStatus.Granted)
			{
				status = await Permissions.RequestAsync<Permissions.Camera>();
			}
		}
	}
}
