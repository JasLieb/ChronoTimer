using ChronoTimer.Core.ViewModels.ChronoSelection;
using ChronoTimer.Core.ViewModels.Setup;

namespace ChronoTimer.Maui.Pages;

public partial class SetupPage : ContentPage
{
    private readonly SetupViewModel _setupViewModel;

    public SetupPage(
        ChronoSelectionViewModel chronoSelectionViewModel,
        SetupViewModel setupViewModel
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

