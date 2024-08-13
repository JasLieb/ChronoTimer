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

		builder.Services
			.AddTransient<IScheduler>(_ => NewThreadScheduler.Default)
			.AddSingleton<INavigator, ShellNavigatorService>()
			.AddSingleton<IChronoTimer, ChronoTimerService>()
			.AddSingleton<ISonificationPlayer, SonificationPlayer>()
			.AddSingleton<DeviceOrientationService>()
			.AddTransient<SetupPage>()
			.AddSingleton<SetupViewModel>()
			.AddTransient<ChronoTimerPage>()
			.AddSingleton<ChronoTimerViewModel>();

		return builder.Build();
	}
}
