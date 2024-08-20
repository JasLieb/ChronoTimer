using ChronoTimer.Core.Models;
using ChronoTimer.Core.Models.ChronoStates;

namespace ChronoTimer.Core.Providers;

public class ColorProvider
{
    private static readonly RGB[] s_redSerie =
        [
            new(255, 107, 107),
            new(249, 98, 97),
            new(243, 90, 87),
            new(237, 81, 77),
            new(231, 72, 67),
            new(224, 62, 57),
            new(218, 53, 47),
            new(211, 42, 36),
            new(204, 30, 25),
            new(197, 13, 13)
        ];

    private static readonly RGB[] s_greenSerie =
        [
            new (167, 205, 157),
            new (162, 205, 153),
            new (156, 204, 149),
            new (151, 204, 144),
            new (145, 204, 141),
            new (138, 203, 137),
            new (132, 203, 133),
            new (125, 203, 130),
            new (117, 202, 126),
            new (109, 202, 123)
        ];

    public static RGB? GetStateColor(GenericChronoState chrono) =>
        (chrono.IsUrgent ?? false)
        ? GetRBGFromSerie(s_redSerie, chrono)
        : GetRBGFromSerie(s_greenSerie, chrono);

    private static RGB GetRBGFromSerie(RGB[] serie, GenericChronoState chronoState)
    {
        var colorIndex = Convert.ToInt32(
            Math.Floor(
                (chronoState.OriginalTime - chronoState.RemainingTime)
                / chronoState.OriginalTime * 10
            )
        );
        return colorIndex < serie.Length - 1
            ? serie[colorIndex]
            : serie.Last();
    }
}
