using Foundation;
using UIKit;

namespace ChronoTimer.Maui;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        return base.FinishedLaunching(application, launchOptions);
    }
}
