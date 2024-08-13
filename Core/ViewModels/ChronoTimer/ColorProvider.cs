namespace ChronoTimer.Core;

public class ColorProvider
{
    private static readonly RGB Red = new(254, 27, 0);
    private static readonly RGB Orange = new(0,0,0);
    private static readonly RGB Green = new(0,128,0);
    public static RGB? GetStateColor(ChronoStates state) =>
        state switch
        {
            ChronoStates.ExerciceTime => Red,
            ChronoStates.BreakTime => Green,
            _ or ChronoStates.NotStarted => null
        };
}
