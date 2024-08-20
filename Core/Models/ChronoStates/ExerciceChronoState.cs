namespace ChronoTimer.Core.Models.ChronoStates;

public enum ExerciceChronoStates
{
    NotStarted,
    ExerciceTime,
    BreakTime
}

public record ExerciceChronoState(
    ExerciceChronoStates State,
    TimeSpan RemainingTime,
    TimeSpan OriginalTime
)
{
    public ExerciceChronoState(
        ExerciceChronoStates state = ExerciceChronoStates.NotStarted,
        TimeSpan? remainingTime = null,
        TimeSpan? originalTime = null
    ) : this(state, remainingTime ?? TimeSpan.Zero, originalTime ?? TimeSpan.MaxValue)
    { }
}
