using ChronoTimer.Core.ViewModels.ChronoTimer;

namespace ChronoTimer.Maui.Pages;

public partial class ChronoTimerPage : ContentPage
{
    private readonly ChronoTimerViewModel _chronoTimerViewModel;

    public ChronoTimerPage(
        ChronoTimerViewModel chronoTimerViewModel
    )
    {
        InitializeComponent();
        BindingContext = chronoTimerViewModel;
        _chronoTimerViewModel = chronoTimerViewModel;
    }

    protected override void OnAppearing()
    {
        _chronoTimerViewModel.OnAppearing();
        base.OnAppearing();
    }
}
