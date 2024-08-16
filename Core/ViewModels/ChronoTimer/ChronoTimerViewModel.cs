using ChronoTimer.Core.Models;
using ChronoTimer.Core.Providers;
using ChronoTimer.Core.Services;
using ChronoTimer.Core.Services.ChronoTimer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChronoTimer.Core.ViewModels.ChronoTimer;

public partial class ChronoTimerViewModel : ObservableObject, IDisposable
{
    private readonly IDisposable _chronoStateSubscription;
    private readonly IChronoTimer _chronoTimer;
    private readonly INavigator _navigator;
    private readonly IDeviceOrientationService _deviceOrientation;
    [ObservableProperty]
    private ChronoState _chronoState = new();

    public ChronoTimerViewModel(
        IChronoTimer chronoTimer,
        INavigator navigator,
        IDeviceOrientationService deviceOrientation
    )
    {
        _chronoTimer = chronoTimer;
        _navigator = navigator;
        _deviceOrientation = deviceOrientation;
        _chronoStateSubscription =
            chronoTimer.Chrono.Subscribe(UpdateViewModelState);
    }

    [RelayCommand]
    private void GotoSetupPage()
    {
        _chronoTimer.StopExercice();
        _navigator.GotoSetup();
    }

    private void UpdateViewModelState(ChronoState chrono) =>
        ChronoState = chrono;

    public void OnAppearing() =>
        _deviceOrientation.SetLandscape();

    public void Dispose() =>
        _chronoStateSubscription.Dispose();
}
