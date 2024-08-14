#if __IOS__
using AVFoundation;
#endif
using ChronoTimer.Core.Services;
using Plugin.Maui.Audio;

namespace ChronoTimer.Maui;

public class SonificationPlayer : ISonificationPlayer
{
    public async void Alarm()
    {
        #if __IOS__
            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
        #endif

        using var audioPlayer = AudioManager.Current.CreateAsyncPlayer(
            await FileSystem.OpenAppPackageFileAsync("Alarm.wav")
        );
        audioPlayer.Volume = 0.25;
        await audioPlayer.PlayAsync(new());    
    }
}
