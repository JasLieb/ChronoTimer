using ChronoTimer.Core.ViewModels.ChronoSelection;
using ChronoTimer.Core.ViewModels.Setups;

namespace ChronoTimer.Maui.Pages;

public partial class ExerciceSetupPage : ContentPage
{
    private readonly ExerciceSetupViewModel _setupViewModel;

    public ExerciceSetupPage(
        ChronoSelectionViewModel chronoSelectionViewModel,
        ExerciceSetupViewModel setupViewModel
    )
    {
        InitializeComponent();
        _setupViewModel = setupViewModel;
        BindingContext = _setupViewModel;
    }

    protected override void OnAppearing()
    {
        _setupViewModel.OnAppearing();
        base.OnAppearing();
    }
}

