using Android.Content.PM;
using ChronoTimer.Core;

namespace ChronoTimer.Maui;

public class DroidDeviceOrientationService : IDeviceOrientationService
{
    public void SetPortrait() =>
        UpdateOrientation(ScreenOrientation.Portrait);
    
    public void SetLandscape() =>
        UpdateOrientation(ScreenOrientation.Landscape);
    
    private void UpdateOrientation(ScreenOrientation orientation)
    {
        var currentActivity = ActivityStateManager.Default.GetCurrentActivity();
        if (currentActivity is not null)
            currentActivity.RequestedOrientation = orientation;
    }
}
