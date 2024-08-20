using ChronoTimer.Core.Services;
using ChronoTimer.Core.ViewModels.Validation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChronoTimer.Core.ViewModels.Setups;

public partial class ChessSetupViewModel(
    INavigator navigator,
    IDeviceOrientationService deviceOrientation
) : ObservableObject
{
    private readonly INavigator _navigator = navigator;
    private readonly IDeviceOrientationService _deviceOrientation = deviceOrientation;

    [ObservableProperty]
    private ValidatableObject<TimeSpan> _timePerPlayer = new([new TimeSpanDifferentThanZero()]);

    [ObservableProperty]
    private string? _error;
    
    [ObservableProperty]
    private bool _isErrorsVisible;

    public void OnAppearing()
    {
        _deviceOrientation.SetPortrait();
        ClearErrors();
    }

    [RelayCommand]
    public void StartGame()
    {
        if (TimePerPlayer.Validate())
        {
            _navigator.GotoChessChronoTimer(
                new(TimePerPlayer.Value)
            );
            return;
        }
        
        DisplayErrors();
    }

    private void DisplayErrors()
    {
        Error = TimePerPlayer.Errors.Distinct().First();
        IsErrorsVisible = true;
    }

    private void ClearErrors()
    {
        IsErrorsVisible = false;
        Error = null;
    }

    [RelayCommand]
    public void ChangeChronoType() =>
        _navigator.GotoSelection();
}
