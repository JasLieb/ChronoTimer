using ChronoTimer.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChronoTimer.Core.ViewModels.Setups;

public partial class ChessSetupViewModel(
    INavigator navigator,
    IDeviceOrientationService deviceOrientation
) : ObservableObject
{
    private readonly INavigator _navigator = navigator;
    private readonly IDeviceOrientationService _deviceOrientation = deviceOrientation;

    public void OnAppearing()
    {
        _deviceOrientation.SetPortrait();
    }

    [RelayCommand]
    public void ChangeChronoType() =>
        _navigator.GotoSelection();
}
