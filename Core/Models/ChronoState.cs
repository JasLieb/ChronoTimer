namespace ChronoTimer.Core;

public enum ChronoStates
{
    NotStarted,
    ExerciceTime,
    BreakTime
}

public record ChronoState(
    ChronoStates State = ChronoStates.NotStarted,
    TimeSpan? RemainingTime = null,
    TimeSpan? OriginalTime = null
);