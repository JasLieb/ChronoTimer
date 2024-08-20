namespace ChronoTimer.Core.Models.ChronoStates;

public record GenericChronoState(
    TimeSpan OriginalTime,
    TimeSpan RemainingTime,
    bool? IsUrgent = null
)
{
    public GenericChronoState() : this(TimeSpan.MaxValue, TimeSpan.Zero, null)
    { }
}
