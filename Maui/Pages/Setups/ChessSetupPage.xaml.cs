using ChronoTimer.Core.ViewModels.Setups;

namespace ChronoTimer.Maui.Pages;

public partial class ChessSetupPage : ContentPage
{
    private readonly ChessSetupViewModel _setupViewModel;

    public ChessSetupPage(ChessSetupViewModel setupViewModel)
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
