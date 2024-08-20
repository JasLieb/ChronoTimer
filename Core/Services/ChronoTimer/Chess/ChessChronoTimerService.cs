using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ChronoTimer.Core.Models.ChronoStates;

namespace ChronoTimer.Core.Services.ChronoTimer.Chess;

public class ChessChronoTimerService(
    IScheduler scheduler,
    ISonificationPlayer sonificationPlayer
) : IChessChronoTimer
{
    private static readonly TimeSpan s_period = TimeSpan.FromMilliseconds(100);

    private IDisposable _currentTimeSubscription = Disposable.Empty;
    private readonly IScheduler _scheduler = scheduler;
    private readonly ISonificationPlayer _sonificationPlayer = sonificationPlayer;

    private readonly BehaviorSubject<ChessChronoState> _chronoSubject = new(new());
    public IObservable<ChessChronoState> Chrono => _chronoSubject;

    public void StartGame(TimeSpan playTime) =>
        EmitChronoState(new(playTime));

    public void StopGame() =>
        EmitStop();

    public void NextPlayer()
    {
        _currentTimeSubscription.Dispose();
        var (nextPlayer, nextPlayerRemainingTime) = GetNextPlayer();
        EmitChronoState(
            new(
                nextPlayer,
                _chronoSubject.Value.OriginalTime,
                _chronoSubject.Value.RemainingTimeFirstPlayer,
                _chronoSubject.Value.RemainingTimeSecondPlayer
            )
        );
        StartTimer(
            nextPlayerRemainingTime,
            UpdateChronoState
        );
    }

    private void UpdateChronoState(TimeSpan remainingTime) 
    {
        if(_chronoSubject.Value.State is ChessChronoStates.FirstPlayerTime)
        {
            EmitChronoState(
                _chronoSubject.Value with
                {
                    RemainingTimeFirstPlayer = remainingTime
                }
            );
            return;
        }
        EmitChronoState(
            _chronoSubject.Value with
            {
                RemainingTimeSecondPlayer = remainingTime
            }
        );
    }

    private (ChessChronoStates nextPlayer, TimeSpan nextPlayerRemainingTime) GetNextPlayer() => 
        _chronoSubject.Value.State is ChessChronoStates.SecondPlayerTime or ChessChronoStates.AwaitPlayerStartGame
        ? (ChessChronoStates.FirstPlayerTime, _chronoSubject.Value.RemainingTimeFirstPlayer)
        : (ChessChronoStates.SecondPlayerTime, _chronoSubject.Value.RemainingTimeSecondPlayer);

    private void StartTimer(
        TimeSpan dueTime,
        Action<TimeSpan> notifyRemainingTime
    )
    {
        notifyRemainingTime(dueTime);
        _currentTimeSubscription =
            Observable.Timer(s_period, _scheduler)
            .Repeat()
            .Subscribe(
                time =>
                {
                    notifyRemainingTime(
                        dueTime -= s_period
                    );
                    if (dueTime <= TimeSpan.Zero)
                    {
                        _currentTimeSubscription.Dispose();
                        _sonificationPlayer.Alarm();
                        EmitStop();
                    }
                }
            );
    }

    private void EmitStop()
    {
        _currentTimeSubscription.Dispose();
        EmitChronoState(new());
    }

    private void EmitChronoState(ChessChronoState chronoState) =>
        _chronoSubject.OnNext(chronoState);
}
