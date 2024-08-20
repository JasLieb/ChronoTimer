namespace ChronoTimer.Tests.ViewModels.ChronoTimer;

public class ChessChronoTimerViewModelTests
{
    private readonly Subject<ChessChronoState> _chronoStateSubject = new();
    private readonly IChessChronoTimer _chronoTimer;
    private readonly INavigator _navigator;
    private readonly IDeviceOrientationService _deviceOrientation;
    private readonly ChessChronoTimerViewModel _viewModel;

    public ChessChronoTimerViewModelTests()
    {
        _chronoTimer = Substitute.For<IChessChronoTimer>();
        _navigator = Substitute.For<INavigator>();
        _chronoTimer.Chrono.Returns(_chronoStateSubject);
        _deviceOrientation = Substitute.For<IDeviceOrientationService>();
        _viewModel = new ChessChronoTimerViewModel(_chronoTimer, _navigator, _deviceOrientation);
    }

    [Fact]
    public void OnApplyQueryAttributesShouldSetPortraitOrientation()
    {
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object>());

        _deviceOrientation.Received().SetPortrait();
    }

    [Fact]
    public void OnApplyQueryAttributesShouldStartExercice()
    {
        var playTime = TimeSpan.FromSeconds(10);
        var exerciceArgs = new Dictionary<string, object>()
        {
            { "request", new ChessRequest(playTime)},
        };

        _viewModel.ApplyQueryAttributes(exerciceArgs);

        _chronoTimer.Received().StartGame(playTime);
    }

    [Fact]
    public void NotStartedShouldHaveBothPlayersNullChronoState()
    {
        _chronoStateSubject.OnNext(new(ChessChronoStates.NotStarted));

        _viewModel.FirstPlayerChronoState.Should().BeNull();
        _viewModel.SecondPlayerChronoState.Should().BeNull();
    }

    [Fact]
    public void NotStartedShouldHaveBothPlayersAreDisabled()
    {
        _chronoStateSubject.OnNext(new(ChessChronoStates.NotStarted));

        _viewModel.IsFirstPlayerEnabled.Should().BeFalse();
        _viewModel.IsSecondPlayerEnabled.Should().BeFalse();
    }

    [Fact]
    public void WhenAwaitPlayerStartGameShouldSetOnlySecondPlayerEnabled()
    {
        _chronoStateSubject.OnNext(new(TimeSpan.FromSeconds(1)));

        _viewModel.IsFirstPlayerEnabled.Should().BeFalse();
        _viewModel.IsSecondPlayerEnabled.Should().BeTrue();
    }

    [Fact]
    public void WhenFirstPlayTimerShouldSetOnlyFirstPlayerEnabled()
    {
        _chronoStateSubject.OnNext(new(ChessChronoStates.FirstPlayerTime));

        _viewModel.IsFirstPlayerEnabled.Should().BeTrue();
        _viewModel.IsSecondPlayerEnabled.Should().BeFalse();
    }

    [Fact]
    public void WhenSecondPlayTimerShouldSetOnlySecondPlayerEnabled()
    {
        _chronoStateSubject.OnNext(new(ChessChronoStates.SecondPlayerTime));

        _viewModel.IsFirstPlayerEnabled.Should().BeFalse();
        _viewModel.IsSecondPlayerEnabled.Should().BeTrue();
    }

    [Fact]
    public void ExecuteNextPlayerCommandShouldTriggerNextPlayer()
    {
        _viewModel.NextPlayerCommand.Execute(null);

        _chronoTimer.Received().NextPlayer();
    }

    [Fact]
    public void ExecuteGotoChessSetupPageCommandShouldStopTimer()
    {
        _viewModel.GotoChessSetupPageCommand.Execute(null);
        _chronoTimer.Received().StopGame();
    }

    [Fact]
    public void ExecuteGotoChessSetupPageCommandShouldGotoExerciceSetup()
    {
        _viewModel.GotoChessSetupPageCommand.Execute(null);
        _navigator.Received().GotoChessSetup();
    }
}
