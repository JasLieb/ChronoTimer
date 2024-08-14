using ChronoTimer.Core.Services;

namespace ChronoTimer.Maui;

public class ShellNavigatorService : INavigator
{
    public async void GotoChronoTimer()
    {
        await Shell.Current.GoToAsync("//chrono");
    }

    public async void GotoSetup()
    {
        await Shell.Current.GoToAsync("//setup");
    }
}
