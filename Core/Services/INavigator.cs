namespace ChronoTimer.Core.Services;

public interface INavigator
{
    void GotoSelection();
    void GotoExerciceSetup();
    void GotoChessSetup();
    void GotoChronoTimer(Models.Requests.ExerciceRequest exerciceRequest);
}
