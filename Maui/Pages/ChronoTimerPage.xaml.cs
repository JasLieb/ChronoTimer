using ChronoTimer.Core.ViewModels.ChronoTimer;
using CommunityToolkit.Maui.Extensions;

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
        ChronoTimer.CurrentColorObservable.Subscribe(
            UpdatePageBackgroundColor
        );
    }

    protected override void OnAppearing()
    {
        _chronoTimerViewModel.OnAppearing();
        base.OnAppearing();
    }

    private void UpdatePageBackgroundColor(Color currentColor) => 
        MainThread.BeginInvokeOnMainThread(
            async () =>
                await PageContainer.BackgroundColorTo(currentColor)
        );
}
