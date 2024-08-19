namespace ChronoTimer.Tests.ViewModels;

public class ChronoSelectionViewModelTests
{
    private readonly INavigator _navigator;
    private readonly ChronoSelectionViewModel _viewModel;

    public ChronoSelectionViewModelTests()
    {
        _navigator = Substitute.For<INavigator>();
        _viewModel = new(_navigator);
    }

    [Fact]
    public void TriggerSelectExcerciceCommandShouldGotoExerciceSetup()
    {
        _viewModel.SelectExerciceCommand.Execute(null);
        _navigator.Received().GotoExerciceSetup();
    }
    
    [Fact]
    public void TriggerSelectChessCommandShouldGotoChessSetup()
    {
        _viewModel.SelectChessCommand.Execute(null);
        _navigator.Received().GotoChessSetup();
    }
}
