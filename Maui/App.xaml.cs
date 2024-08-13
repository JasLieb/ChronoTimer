using Microsoft.Maui.Controls;

namespace ChronoTimer.Maui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	protected override async void OnStart()
	{
		await Shell.Current.GoToAsync("//setup");
		base.OnStart();

		PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Media>();
		if (status != PermissionStatus.Granted)
		{
			status = await Permissions.RequestAsync<Permissions.Media>();
		}
	}
}
