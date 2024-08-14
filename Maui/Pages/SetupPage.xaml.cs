using ChronoTimer.Core.ViewModels.Setup;

namespace ChronoTimer.Maui.Pages;

public partial class SetupPage : ContentPage
{
    private readonly SetupViewModel _setupViewModel;

    public SetupPage(
        SetupViewModel setupViewModel
    )
    {
        InitializeComponent();
        BindingContext = setupViewModel;
        _setupViewModel = setupViewModel;
    }

    protected override void OnAppearing()
    {
        _setupViewModel.OnAppearing();
        base.OnAppearing();
    }
}

