using ChronoTimer.Core.ViewModels.ChronoTimer;

namespace ChronoTimer.Maui.Pages.ChronoTimer;

public partial class ChessChronoTimerPage : ContentPage
{
    public ChessChronoTimerPage(
        ChessChronoTimerViewModel chessChronoTimerViewModel
    )
    {
        InitializeComponent();
        BindingContext = chessChronoTimerViewModel;
    }
}
