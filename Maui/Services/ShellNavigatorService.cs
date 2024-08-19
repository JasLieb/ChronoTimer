using ChronoTimer.Core.Models.Requests;
using ChronoTimer.Core.Services;

namespace ChronoTimer.Maui.Services;

public class ShellNavigatorService : INavigator
{
    public async void GotoSelection() => 
        await Shell.Current.GoToAsync("//selection");

    public async void GotoExerciceSetup() => 
        await Shell.Current.GoToAsync("//exerciceSetup");

    public async void GotoChessSetup() =>
        await Shell.Current.GoToAsync("//chessSetup");

    public async void GotoChronoTimer(ExerciceRequest request) => 
        await Shell.Current.GoToAsync("//chrono", MakeNavigationArgs(request));
    
    private Dictionary<string, object> MakeNavigationArgs(ExerciceRequest request) => 
        new() { {"request", request} };
}
