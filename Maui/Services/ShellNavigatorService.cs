using ChronoTimer.Core.Services;

namespace ChronoTimer.Maui.Services;

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

    public async void GotoSelection()
    {
        await Shell.Current.GoToAsync("//selection");
    }
}
