using ChronoTimer.Core.Services;

namespace ChronoTimer.Maui;

public partial class App : Application
{
    private readonly INavigator _navigator;

    public App(
        INavigator navigator
    )
	{
		InitializeComponent();

		MainPage = new AppShell();
        _navigator = navigator;
    }

	protected override async void OnStart()
	{
		_navigator.GotoSetup();
        
		PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Media>();
		if (status != PermissionStatus.Granted)
		{
			status = await Permissions.RequestAsync<Permissions.Media>();
		}
        
		base.OnStart();

	}
}
