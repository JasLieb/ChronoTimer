using ChronoTimer.Core.Models.ChronoStates;

namespace ChronoTimer.Core.Services.ChronoTimer.Chess;

public interface IChessChronoTimer
{
    IObservable<ChessChronoState> Chrono { get; }

    void StartGame(TimeSpan playTime);
    void NextPlayer();
    void StopGame();
}
