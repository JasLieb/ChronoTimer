using System.Reactive.Concurrency;
using ChronoTimer.Core.Services;
using ChronoTimer.Core.Services.ChronoTimer;
using ChronoTimer.Core.ViewModels;
using ChronoTimer.Core.ViewModels.ChronoTimer;
using ChronoTimer.Maui.Pages;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace ChronoTimer.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiCommunityToolkit()
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

#if IOS
		builder.Services
			.AddSingleton<IDeviceOrientationService, Platforms.iOS.Services.IOSDeviceOrientationService>();
#elif ANDROID
		builder.Services
			.AddSingleton<IDeviceOrientationService, Platforms.Android.Services.DroidDeviceOrientationService>();
#else
		builder.Services
			.AddSingleton<IDeviceOrientationService, Services.NullDeviceOrientationService>();
#endif

        builder.Services
			.AddTransient<IScheduler>(_ => NewThreadScheduler.Default)
			.AddSingleton<INavigator, ShellNavigatorService>()
			.AddSingleton<IChronoTimer, ChronoTimerService>()
			.AddSingleton<ISonificationPlayer, SonificationPlayer>()
			.AddTransient<SetupPage>()
			.AddSingleton<SetupViewModel>()
			.AddTransient<ChronoTimerPage>()
			.AddSingleton<ChronoTimerViewModel>();

		return builder.Build();
	}
}
