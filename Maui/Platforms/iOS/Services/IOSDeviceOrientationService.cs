using ChronoTimer.Core.Services;
using Foundation;
using UIKit;

namespace ChronoTimer.Maui.Platforms.iOS.Services;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Version 16.0 already checked")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1422:Validate platform compatibility", Justification = "Version 16.0 already checked")]
public class IOSDeviceOrientationService : IDeviceOrientationService
{
    public void SetPortrait()
    {
        UpdateIosOrientation(UIInterfaceOrientationMask.Portrait);
    }
    
    public void SetLandscape()
    {
        UpdateIosOrientation(UIInterfaceOrientationMask.Landscape);
    }

    private void UpdateIosOrientation(UIInterfaceOrientationMask orientationMask)
    {
        if (UIDevice.CurrentDevice.CheckSystemVersion(16, 0))
        {
            if (
                UIApplication.SharedApplication
                .ConnectedScenes
                .ToArray()[0] is UIWindowScene windowScene
            )
            {
                var nav = UIApplication.SharedApplication.KeyWindow?.RootViewController;
                if (nav != null)
                {
                    nav.SetNeedsUpdateOfSupportedInterfaceOrientations();

                    windowScene.RequestGeometryUpdate(
                        new UIWindowSceneGeometryPreferencesIOS(orientationMask),
                        error => { }
                    );
                }
            }
            return;
        }
        UIDevice.CurrentDevice.SetValueForKey(
            new NSNumber((int)orientationMask), 
            new NSString("orientation")
        );
    }
}
