using ChronoTimer.Core.Models;

namespace ChronoTimer.Core.ViewModels.ChronoTimer;

public class ColorProvider
{
    private static readonly RGB s_red = new(255, 107, 107);
    private static readonly RGB s_orange = new(0, 0, 0);
    private static readonly RGB s_green = new(107, 203, 119);
    public static RGB? GetStateColor(ChronoStates state) =>
        state switch
        {
            ChronoStates.ExerciceTime => s_red,
            ChronoStates.BreakTime => s_green,
            _ or ChronoStates.NotStarted => null
        };
}
