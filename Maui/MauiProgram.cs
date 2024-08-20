using System.Reactive.Concurrency;
using ChronoTimer.Core.Services;
using ChronoTimer.Core.ViewModels.Setups;
using ChronoTimer.Core.ViewModels.ChronoTimer;
using ChronoTimer.Core.ViewModels.ChronoSelection;
using ChronoTimer.Maui.Pages;
using ChronoTimer.Maui.Services;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ChronoTimer.Core.Services.ChronoTimer.Exercice;
using ChronoTimer.Core.Services.ChronoTimer.Chess;
using ChronoTimer.Maui.Pages.ChronoTimer;

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
			.AddSingleton<IExerciceChronoTimer, ExerciceChronoTimerService>()
			.AddSingleton<IChessChronoTimer, ChessChronoTimerService>()
			.AddSingleton<ISonificationPlayer, SonificationPlayer>()
			.AddTransient<ChronoSelectionPage>()
			.AddSingleton<ChronoSelectionViewModel>()
			.AddTransient<ExerciceSetupPage>()
			.AddSingleton<ExerciceSetupViewModel>()
			.AddTransient<ChessSetupPage>()
			.AddTransient<ChessSetupViewModel>()
			.AddTransient<ExerciceChronoTimerPage>()
			.AddSingleton<ExerciceChronoTimerViewModel>()
			.AddTransient<ChessChronoTimerPage>()
			.AddSingleton<ChessChronoTimerViewModel>();

		return builder.Build();
	}
}
