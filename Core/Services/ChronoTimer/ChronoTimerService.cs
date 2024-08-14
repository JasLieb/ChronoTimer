using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ChronoTimer.Core.Models;

namespace ChronoTimer.Core.Services.ChronoTimer;

public class ChronoTimerService(
    IScheduler scheduler,
    ISonificationPlayer sonificationPlayer
) : IChronoTimer
{
    private static readonly TimeSpan _period = TimeSpan.FromMilliseconds(100);
    private readonly IScheduler _scheduler = scheduler;
    private readonly ISonificationPlayer _sonificationPlayer = sonificationPlayer;

    private IDisposable _currentTimeSubscription = Disposable.Empty;

    public IObservable<ChronoState> Chrono => _chronoSubject;
    private readonly BehaviorSubject<ChronoState> _chronoSubject = new(new());

    public void StartExercice(TimeSpan exerciceTime, TimeSpan breakTime)
    {
        StartTimer(
            exerciceTime,
            remainingTime => EmitExerciceTime(remainingTime, exerciceTime),
            () => StartBreak(exerciceTime, breakTime)
        );
    }

    public void StopExercice() =>
        EmitStop();

    private void StartBreak(TimeSpan exerciceTime, TimeSpan breakTime) =>
        StartTimer(
            breakTime,
            remaining => EmitBreakTime(remaining, breakTime),
            () => StartExercice(exerciceTime, breakTime)
        );

    private void StartTimer(
        TimeSpan dueTime,
        Action<TimeSpan> notifyRemainingTime,
        Action elapsedAction
    )
    {
        notifyRemainingTime(dueTime);
        _currentTimeSubscription =
            Observable.Timer(_period, _scheduler)
            .Repeat()
            .Subscribe(
                time =>
                {
                    notifyRemainingTime(dueTime -= _period);
                    if (dueTime <= TimeSpan.Zero)
                    {
                        _currentTimeSubscription.Dispose();
                        _sonificationPlayer.Alarm();
                        elapsedAction();
                    }
                }
            );
    }

    private void EmitExerciceTime(
        TimeSpan remainingTime,
        TimeSpan originalTime
    ) =>
        EmitChronoState(
            new(ChronoStates.ExerciceTime, remainingTime, originalTime)
        );

    private void EmitBreakTime(
        TimeSpan remainingTime,
        TimeSpan originalTime
    ) =>
        EmitChronoState(
            new(ChronoStates.BreakTime, remainingTime, originalTime)
        );

    private void EmitStop()
    {
        _currentTimeSubscription.Dispose();
        EmitChronoState(new());
    }

    private void EmitChronoState(ChronoState chronoState) =>
        _chronoSubject.OnNext(chronoState);
}
