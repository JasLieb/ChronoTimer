using ChronoTimer.Core.ViewModels.ChronoSelection;

namespace ChronoTimer.Maui.Pages;

public partial class ChronoSelectionPage : ContentPage
{
    public ChronoSelectionPage(
        ChronoSelectionViewModel chronoSelectionViewModel
    )
    {
        InitializeComponent();
        BindingContext = chronoSelectionViewModel;
    }
}
