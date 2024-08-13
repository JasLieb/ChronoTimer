using ChronoTimer.Core;

namespace ChronoTimer.Maui;

public partial class ChronoTimerPage : ContentPage
{
    private readonly DeviceOrientationService _deviceOrientationService;

	public ChronoTimerPage(
		ChronoTimerViewModel chronoTimerViewModel,
		DeviceOrientationService deviceOrientationService
	)
	{
		InitializeComponent();
		BindingContext = chronoTimerViewModel;
        _deviceOrientationService = deviceOrientationService;
    }

    protected override void OnAppearing()
    {
		_deviceOrientationService.SetLandspace();
        base.OnAppearing();
    }
}