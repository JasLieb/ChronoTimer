using ChronoTimer.Core.ViewModels.ChronoTimer;
using CommunityToolkit.Maui.Extensions;

namespace ChronoTimer.Maui.Pages;

public partial class ChronoTimerPage : ContentPage, IDisposable
{
    private readonly IDisposable _disposable;

    public ChronoTimerPage(
        ChronoTimerViewModel chronoTimerViewModel
    )
    {
        InitializeComponent();
        BindingContext = chronoTimerViewModel;
        _disposable = 
            ChronoTimer.CurrentColorObservable.Subscribe(
                UpdatePageBackgroundColor
            );
    }

    private void UpdatePageBackgroundColor(Color currentColor) =>
        MainThread.BeginInvokeOnMainThread(
            async () =>
                await PageContainer.BackgroundColorTo(currentColor)
        );

    public void Dispose() => _disposable.Dispose();
}
