using ChronoTimer.Core.Services;

namespace ChronoTimer.Maui.Services;

public class NullDeviceOrientationService : IDeviceOrientationService
{
    public void SetPortrait()
    { }

    public void SetLandscape()
    { }
}
