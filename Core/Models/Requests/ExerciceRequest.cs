namespace ChronoTimer.Core.Models.Requests;

public record ExerciceRequest(
    TimeSpan ExerciceTime,
    TimeSpan BreakTime
);
