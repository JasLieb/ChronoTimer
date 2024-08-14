// #if __IOS__
// using Foundation;
// using UIKit;
// #endif

using ChronoTimer.Core;

namespace ChronoTimer.Maui;

public class NullDeviceOrientationService : IDeviceOrientationService
{
    public void SetPortrait()
    {
        // #if __ANDROID__
        //     var currentActivity = ActivityStateManager.Default.GetCurrentActivity();
        //     if (currentActivity is not null)
        //     {
        //         if (_androidDisplayOrientationMap.TryGetValue(displayOrientation, out ScreenOrientation screenOrientation))
        //             currentActivity.RequestedOrientation = screenOrientation;
        //     }
        // #endif
        // #if __IOS__
        //     UpdateIosOrientation(UIInterfaceOrientationMask.Portrait);
        // #endif
    }
    
    public void SetLandscape()
    {
        // #if __IOS__
        //     UpdateIosOrientation(UIInterfaceOrientationMask.Landscape);
        // #endif
    }
    
    // #if __IOS__

    // private void UpdateIosOrientation(UIInterfaceOrientationMask orientationMask)
    // {
    //     if (UIDevice.CurrentDevice.CheckSystemVersion(16, 0))
    //     {
    //         if (
    //             UIApplication.SharedApplication
    //             .ConnectedScenes
    //             .ToArray()[0] is UIWindowScene windowScene
    //         )
    //         {
    //             var nav = UIApplication.SharedApplication.KeyWindow?.RootViewController;
    //             if (nav != null)
    //             {
    //                 // Tell the os that we changed orientations so it knows to call GetSupportedInterfaceOrientations again
    //                 nav.SetNeedsUpdateOfSupportedInterfaceOrientations();

    //                 windowScene.RequestGeometryUpdate(
    //                     new UIWindowSceneGeometryPreferencesIOS(orientationMask),
    //                     error => { }
    //                 );
    //             }
    //         }
    //     }
    //     else
    //     {
    //         UIDevice.CurrentDevice.SetValueForKey(
    //             new NSNumber((int)orientationMask), 
    //             new NSString("orientation")
    //         );
    //     }
    // }
    // #endif
}
