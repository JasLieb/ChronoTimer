using ChronoTimer.Core.Models;

namespace ChronoTimer.Core.Services.ChronoTimer;

public interface IChronoTimer
{
    IObservable<ChronoState> Chrono { get; }

    void StartExercice(TimeSpan exerciceTime, TimeSpan breakTime);
    void StopExercice();
}
