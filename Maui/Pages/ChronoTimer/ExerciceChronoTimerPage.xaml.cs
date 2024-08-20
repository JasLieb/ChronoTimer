using ChronoTimer.Core.ViewModels.ChronoTimer;
using CommunityToolkit.Maui.Extensions;

namespace ChronoTimer.Maui.Pages.ChronoTimer;

public partial class ExerciceChronoTimerPage : ContentPage, IDisposable
{
    private readonly IDisposable _disposable;

    public ExerciceChronoTimerPage(
        ExerciceChronoTimerViewModel chronoTimerViewModel
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
