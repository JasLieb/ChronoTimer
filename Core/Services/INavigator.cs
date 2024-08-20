using ChronoTimer.Core.Models.Requests;

namespace ChronoTimer.Core.Services;

public interface INavigator
{
    void GotoSelection();
    void GotoExerciceSetup();
    void GotoChessSetup();
    void GotoExerciceChronoTimer(ExerciceRequest exerciceRequest);
    void GotoChessChronoTimer(ChessRequest chessRequest);
}
