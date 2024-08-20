namespace ChronoTimer.Tests.ViewModels.Setups;

public class ChessSetupViewModelTests
{
    private readonly INavigator _navigator;
    private readonly IDeviceOrientationService _deviceOrientation;
    private readonly ChessSetupViewModel _viewModel;

    public ChessSetupViewModelTests()
    {
        _navigator = Substitute.For<INavigator>();
        _deviceOrientation = Substitute.For<IDeviceOrientationService>();
        _viewModel = new ChessSetupViewModel(_navigator, _deviceOrientation);
    }

    [Fact]
    public void OnAppearingShouldSetPortraitOrientation()
    {
        _viewModel.OnAppearing();

        _deviceOrientation.Received().SetPortrait();
    }

    [Fact]
    public void OnAppearingShouldResetErrors()
    {
        _viewModel.OnAppearing();

        _viewModel.Error.Should().BeNull();
        _viewModel.IsErrorsVisible.Should().BeFalse();
    }

    [Fact]
    public void ASelectedTimeIsZeroShouldCannotExecuteStartExerciceCommand()
    {
        _viewModel.TimePerPlayer.Value = TimeSpan.FromSeconds(0);

        _viewModel.StartGameCommand.Execute(null);

        _navigator.DidNotReceive().GotoChessChronoTimer(Arg.Any<ChessRequest>());
    }
    
    [Fact]
    public void ASelectedTimeIsZeroShouldDisplayError()
    {
        _viewModel.TimePerPlayer.Value = TimeSpan.FromSeconds(0);

        _viewModel.StartGameCommand.Execute(null);

        _viewModel.Error.Should().Be("Time cannot be Zero");
        _viewModel.IsErrorsVisible.Should().BeTrue();
    }

    [Fact]
    public void OnStartExerciceCommandShouldStartExercice()
    {
        var timePerPlayer = TimeSpan.FromSeconds(10);
        _viewModel.TimePerPlayer.Value = timePerPlayer;
        
        _viewModel.StartGameCommand.Execute(null);
        
        _navigator.Received().GotoChessChronoTimer(
            Arg.Is<ChessRequest>(
                request => request.TimePerPlayer == timePerPlayer
            )
        );
    }

    [Fact]
    public void OnChangeChronoTypeCommandShouldGotoSelection()
    {
        _viewModel.ChangeChronoTypeCommand.Execute(null);

        _navigator.Received().GotoSelection();
    }
}
