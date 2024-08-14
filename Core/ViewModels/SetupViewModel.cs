using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ChronoTimer.Core;

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
    private TimeSpan _exerciceTime = TimeSpan.FromSeconds(10);
    
    [ObservableProperty]
    private TimeSpan _breakTime = TimeSpan.FromSeconds(5);

    [RelayCommand]
    private void StartExercice()
    {
        _chronoTimer.StartExercice(
            ExerciceTime,
            BreakTime
        );
        _navigator.GotoChronoTimer();
    }

    public void OnAppearing()
    {
        _deviceOrientation.SetPortrait();
    }
}
