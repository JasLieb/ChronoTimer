namespace ChronoTimer.Core;

public interface IChronoTimer
{
    IObservable<ChronoState> Chrono { get; }
    
    void StartExercice(TimeSpan exerciceTime, TimeSpan breakTime);
    void StopExercice();
}
