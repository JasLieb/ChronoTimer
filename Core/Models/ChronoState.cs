namespace ChronoTimer.Core.Models;

public enum ChronoStates
{
    NotStarted,
    ExerciceTime,
    BreakTime
}

public record ChronoState(
    ChronoStates State,
    TimeSpan RemainingTime,
    TimeSpan OriginalTime
)
{
    public ChronoState(
        ChronoStates state = ChronoStates.NotStarted,
        TimeSpan? remainingTime = null,
        TimeSpan? originalTime = null
    ) : this(state, remainingTime ?? TimeSpan.Zero, originalTime ?? TimeSpan.MaxValue) 
    {}
}
