using ChronoTimer.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChronoTimer.Core.ViewModels.ChronoSelection;

public partial class ChronoSelectionViewModel(
    INavigator navigator
) : ObservableObject
{
    private readonly INavigator _navigator = navigator;

    [RelayCommand]
    public void SelectExercice() =>
        _navigator.GotoExerciceSetup();
    
    [RelayCommand]
    public void SelectChess() =>
        _navigator.GotoChessSetup();
}
