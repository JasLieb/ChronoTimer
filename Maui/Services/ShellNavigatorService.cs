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

    public async void GotoExerciceChronoTimer(ExerciceRequest request) => 
        await Shell.Current.GoToAsync(
            "//exerciceChrono", 
            new Dictionary<string, object>() { {"request", request} }
        );
    
    public async void GotoChessChronoTimer(ChessRequest request) => 
        await Shell.Current.GoToAsync(
            "//chessChrono", 
            new Dictionary<string, object>() { {"request", request} }
        );
}
