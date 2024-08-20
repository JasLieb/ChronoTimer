namespace ChronoTimer.Core.Models.ChronoStates;

public enum ChessChronoStates
{
    NotStarted,
    AwaitPlayerStartGame,
    FirstPlayerTime,
    SecondPlayerTime
}

public record ChessChronoState(
    ChessChronoStates State,
    TimeSpan OriginalTime,
    TimeSpan RemainingTimeFirstPlayer,
    TimeSpan RemainingTimeSecondPlayer
)
{
    public ChessChronoState(
        TimeSpan originalTime
    ) : this(
        ChessChronoStates.AwaitPlayerStartGame, 
        originalTime,
        originalTime,
        originalTime
    )
    {}
    
    public ChessChronoState(
        ChessChronoStates state = ChessChronoStates.NotStarted,
        TimeSpan? originalTime = null,
        TimeSpan? remainingTimeFirstPlayer = null,
        TimeSpan? remainingTimeSecondPlayer = null
    ) : this(
        state, 
        originalTime ?? TimeSpan.MaxValue, 
        remainingTimeFirstPlayer ?? TimeSpan.Zero, 
        remainingTimeSecondPlayer ?? TimeSpan.Zero
    )
    {}
}
