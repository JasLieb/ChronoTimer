using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ChronoTimer.Core;

public partial class SetupViewModel(
    IChronoTimer chronoTimer,
    INavigator navigator
) : ObservableObject
{
    private readonly IChronoTimer _chronoTimer = chronoTimer;
    private readonly INavigator _navigator = navigator;

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
}
