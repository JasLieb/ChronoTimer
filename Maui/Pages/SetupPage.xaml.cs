using ChronoTimer.Core;

namespace ChronoTimer.Maui;

public partial class SetupPage : ContentPage
{
    private readonly DeviceOrientationService _deviceOrientationService;

    public SetupPage(
		SetupViewModel setupViewModel,
		DeviceOrientationService deviceOrientationService
	)
	{
		InitializeComponent();
		BindingContext = setupViewModel;
        _deviceOrientationService = deviceOrientationService;
    }

    protected override void OnAppearing()
    {
		_deviceOrientationService.SetPortrait();
        base.OnAppearing();
    }
}

