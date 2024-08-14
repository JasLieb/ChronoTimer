using System.Reactive.Concurrency;
using ChronoTimer.Core;
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

#if __IOS__
		builder.Services
			.AddSingleton<IDeviceOrientationService, IOSDeviceOrientationService>();
#elif __ANDROID__
		builder.Services
			.AddSingleton<IDeviceOrientationService, DroidDeviceOrientationService>();
#else
		builder.Services
			.AddSingleton<IDeviceOrientationService, NullDeviceOrientationService>();
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
