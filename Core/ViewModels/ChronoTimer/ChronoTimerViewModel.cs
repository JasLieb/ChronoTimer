using ChronoTimer.Core.Models;
using ChronoTimer.Core.Services;
using ChronoTimer.Core.Services.ChronoTimer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChronoTimer.Core.Models.Requests;

namespace ChronoTimer.Core.ViewModels.ChronoTimer;

public partial class ChronoTimerViewModel : ObservableObject, IDisposable, IQueryAttributable
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
    
    public void ApplyQueryAttributes(IDictionary<string, object> query) 
    {
        _deviceOrientation.SetLandscape();
        if (
            query.TryGetValue("request", out var value) 
            && value is ExerciceRequest request
        )
        {
            _chronoTimer.StartExercice(request.ExerciceTime, request.BreakTime);
        }
    }

    [RelayCommand]
    private void GotoExerciceSetupPage()
    {
        _chronoTimer.StopExercice();
        _navigator.GotoExerciceSetup();
    }

    private void UpdateViewModelState(ChronoState chrono) =>
        ChronoState = chrono;

    public void Dispose() =>
        _chronoStateSubscription.Dispose();
}
