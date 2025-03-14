using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.ApplicationModel;
using Plugin.LocalNotification;
using TurfApp.MVVM.View;

#if ANDROID
using Android;
using Android.App;
using Android.OS;
using Android.Content;
using Android.Content.PM;
#endif

namespace TurfApp
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseLocalNotification()
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
					MainThread.BeginInvokeOnMainThread(async () =>
					{
						await RequestCameraPermission();
						await RequestNotificationPermission(activity);
					});
				}));
			});
#endif

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}

		private static async Task RequestCameraPermission()
		{
			var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

			if (status != PermissionStatus.Granted)
			{
				status = await Permissions.RequestAsync<Permissions.Camera>();
			}
		}

#if ANDROID
		private static async Task RequestNotificationPermission(Activity activity)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu) 
			{
				if (activity.CheckSelfPermission(Manifest.Permission.PostNotifications) != Permission.Granted)
				{
					activity.RequestPermissions(new string[] { Manifest.Permission.PostNotifications }, 0);
				}
			}
		}
#endif
	}
}
