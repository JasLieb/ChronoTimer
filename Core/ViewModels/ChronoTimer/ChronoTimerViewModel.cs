using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChronoTimer.Core;

public partial class ChronoTimerViewModel : ObservableObject, IDisposable
{
    private readonly IDisposable _chronoStateSubscription;
    private readonly IChronoTimer _chronoTimer;
    private readonly INavigator _navigator;

    [ObservableProperty]
    private ChronoState _chronoState = new();
    
    [ObservableProperty]
    private RGB? _timerColor;

    public ChronoTimerViewModel(
        IChronoTimer chronoTimer, 
        INavigator navigator
    )
    {
        _chronoTimer = chronoTimer;
        _navigator = navigator;
        _chronoStateSubscription =
            chronoTimer.Chrono.Subscribe(UpdateViewModelState);
    }

    [RelayCommand]
    private void GotoSetupPage()
    {
        _chronoTimer.StopExercice();
        _navigator.GotoSetup();
    }

    private void UpdateViewModelState(ChronoState chrono)
    {
        ChronoState = chrono;
        TimerColor = ColorProvider.GetStateColor(chrono.State);
    }

    public void Dispose() =>
        _chronoStateSubscription.Dispose();
}
