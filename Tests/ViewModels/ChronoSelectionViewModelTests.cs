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
    public void TriggerSelectExcerciceCommandShouldGotoSetup()
    {
        _viewModel.SelectExerciceCommand.Execute(null);
        _navigator.Received().GotoSetup();
    }
}
