using ChronoTimer.Core.Models.ChronoStates;

namespace ChronoTimer.Core.Services.ChronoTimer.Exercice;

public interface IExerciceChronoTimer
{
    IObservable<ExerciceChronoState> Chrono { get; }

    void StartExercice(TimeSpan exerciceTime, TimeSpan breakTime);
    void StopExercice();
}
