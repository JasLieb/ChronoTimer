using ChronoTimer.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChronoTimer.Core.ViewModels.ChronoSelection;

public partial class ChronoSelectionViewModel : ObservableObject
{
    private readonly INavigator _navigator;

    public ChronoSelectionViewModel(INavigator navigator) 
    {
        _navigator = navigator;
    }

    [RelayCommand]
    public void SelectExercice() =>
        _navigator.GotoSetup();
}
