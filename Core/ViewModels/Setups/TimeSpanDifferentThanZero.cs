using ChronoTimer.Core.ViewModels.Validation;

namespace ChronoTimer.Core.ViewModels.Setups;

public class TimeSpanDifferentThanZero : IValidationRule<TimeSpan>
{
    public string ValidationMessage {get; set;} = "Time cannot be Zero";

    public bool Check(TimeSpan value) => 
        value != TimeSpan.Zero;
}
