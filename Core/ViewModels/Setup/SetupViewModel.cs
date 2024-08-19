using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using ChronoTimer.Core.Services.ChronoTimer;
using ChronoTimer.Core.Services;
using ChronoTimer.Core.ViewModels.Validation;

namespace ChronoTimer.Core.ViewModels.Setup;

public partial class SetupViewModel(
    IChronoTimer chronoTimer,
    INavigator navigator,
    IDeviceOrientationService deviceOrientation
) : ObservableObject
{
    private readonly IChronoTimer _chronoTimer = chronoTimer;
    private readonly INavigator _navigator = navigator;
    private readonly IDeviceOrientationService _deviceOrientation = deviceOrientation;

    [ObservableProperty]
    private ValidatableObject<TimeSpan> _exerciceTime = new([new TimeSpanDifferentThanZero()]);

    [ObservableProperty]
    private ValidatableObject<TimeSpan> _breakTime = new([new TimeSpanDifferentThanZero()]);
    
    [ObservableProperty]
    private string? _error;
    
    [ObservableProperty]
    private bool _isErrorsVisible;

    private bool _canStartExercice => 
        ExerciceTime.Validate() && BreakTime.Validate();

    [RelayCommand]
    private void StartExercice()
    {
        if (!_canStartExercice)
        {
            DisplayErrors();
            return;
        }

        _chronoTimer.StartExercice(
            ExerciceTime.Value,
            BreakTime.Value
        );
        _navigator.GotoChronoTimer();
    }

    [RelayCommand]
    public void ChangeChronoType() =>
        _navigator.GotoSelection();

    public void OnAppearing()
    {
        ClearErrors();
        _deviceOrientation.SetPortrait();
    }
    
    private void DisplayErrors()
    {
        Error = ExerciceTime.Errors
            .Concat(BreakTime.Errors)
            .Distinct()
            .First();
        IsErrorsVisible = true;
    }

    private void ClearErrors()
    {
        IsErrorsVisible = false;
        Error = null;
    }
}
