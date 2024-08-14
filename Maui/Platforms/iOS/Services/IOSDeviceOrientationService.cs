#if __IOS__
using ChronoTimer.Core.Services;
using Foundation;
using UIKit;
#endif

namespace ChronoTimer.Maui.Platforms.iOS.Services;

public class IOSDeviceOrientationService : IDeviceOrientationService
{
    public void SetPortrait()
    {
        #if __IOS__
            UpdateIosOrientation(UIInterfaceOrientationMask.Portrait);
        #endif
    }
    
    public void SetLandscape()
    {
        #if __IOS__
            UpdateIosOrientation(UIInterfaceOrientationMask.Landscape);
        #endif
    }
    
    #if __IOS__

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
                    // Tell the os that we changed orientations so it knows to call GetSupportedInterfaceOrientations again
                    nav.SetNeedsUpdateOfSupportedInterfaceOrientations();

                    windowScene.RequestGeometryUpdate(
                        new UIWindowSceneGeometryPreferencesIOS(orientationMask),
                        error => { }
                    );
                }
            }
        }
        else
        {
            UIDevice.CurrentDevice.SetValueForKey(
                new NSNumber((int)orientationMask), 
                new NSString("orientation")
            );
        }
    }
    #endif
}
