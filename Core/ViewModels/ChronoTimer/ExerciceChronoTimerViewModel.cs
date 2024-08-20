using ChronoTimer.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChronoTimer.Core.Models.Requests;
using ChronoTimer.Core.Services.ChronoTimer.Exercice;
using ChronoTimer.Core.Models.ChronoStates;

namespace ChronoTimer.Core.ViewModels.ChronoTimer;

public partial class ExerciceChronoTimerViewModel : ObservableObject, IDisposable, IQueryAttributable
{
    private readonly IDisposable _chronoStateSubscription;
    private readonly IExerciceChronoTimer _chronoTimer;
    private readonly INavigator _navigator;
    private readonly IDeviceOrientationService _deviceOrientation;
    
    [ObservableProperty]
    private ExerciceChronoState _chronoState = new();

    public ExerciceChronoTimerViewModel(
        IExerciceChronoTimer chronoTimer,
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

    private void UpdateViewModelState(ExerciceChronoState chrono) =>
        ChronoState = chrono;

    public void Dispose() =>
        _chronoStateSubscription.Dispose();
}
