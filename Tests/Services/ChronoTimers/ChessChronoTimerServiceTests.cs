namespace ChronoTimer.Tests.Services.ChronoTimers;

public class ChessChronoTimerServiceTests : IDisposable
{
    private readonly TimeSpan _playTime = TimeSpan.FromSeconds(3);

    private readonly ChessChronoTimerService _timer;
    private readonly TestScheduler _scheduler = new();
    private readonly ISonificationPlayer _sonificationPlayer = Substitute.For<ISonificationPlayer>();
    private readonly IDisposable _chronoSubcription;
    private ChessChronoState? _currentChrono;

    public ChessChronoTimerServiceTests()
    {
        _timer = new ChessChronoTimerService(_scheduler, _sonificationPlayer);
        _chronoSubcription = _timer.Chrono.Subscribe(
            chrono => _currentChrono = chrono
        );
    }

    [Fact]
    public void NotStartedTimerShouldHaveNotStartedState()
    {
        _currentChrono!.State.Should().Be(ChessChronoStates.NotStarted);
    }

    [Fact]
    public void StartGameShouldUpdateChronoWithAwaitPlayerStartGame()
    {
        _timer.StartGame(_playTime);

        _currentChrono!.State.Should().Be(ChessChronoStates.AwaitPlayerStartGame);
        _currentChrono.OriginalTime.Should().Be(_playTime);
        _currentChrono.RemainingTimeFirstPlayer.Should().Be(_playTime);
        _currentChrono.RemainingTimeSecondPlayer.Should().Be(_playTime);
    }
    
    [Fact]
    public void StartGameAndNextPlayerShouldUpdateChronoWithFirstPlayerTimeState()
    {
        StartGameAndNextPlayer();

        _currentChrono!.State.Should().Be(ChessChronoStates.FirstPlayerTime);
    }

    [Fact]
    public void StartGameAndNextPlayerShouldUpdateChronoWithFirstPlayerTime()
    {
        StartGameAndNextPlayer();
        var elapsedTime = TimeSpan.FromSeconds(1);

        _scheduler.AdvanceBy(elapsedTime.Ticks);

        _currentChrono!.State.Should().Be(ChessChronoStates.FirstPlayerTime);
        _currentChrono.OriginalTime.Should().Be(_playTime);
        _currentChrono.RemainingTimeFirstPlayer.Should().Be(
            _playTime - elapsedTime
        );
        _currentChrono.RemainingTimeSecondPlayer.Should().Be(
            _playTime
        );
    }
    
    [Fact]
    public void RunGameAndNextPlayerShouldUpdateChronoWithSecondPlayerTime()
    {
        StartGameAndNextPlayer();
        _timer.NextPlayer();
        var elapsedTime = TimeSpan.FromSeconds(1);

        _scheduler.AdvanceBy(elapsedTime.Ticks);

        _currentChrono!.State.Should().Be(ChessChronoStates.SecondPlayerTime);
        _currentChrono.OriginalTime.Should().Be(_playTime);
        _currentChrono.RemainingTimeSecondPlayer.Should().Be(
            _playTime - elapsedTime
        );
        _currentChrono.RemainingTimeFirstPlayer.Should().Be(
            _playTime
        );
    }

    [Fact]
    public void ReachEndOfTimeTimeShouldPlaySonification()
    {
        StartGameAndNextPlayer();

        _scheduler.AdvanceBy(_playTime.Ticks);

        _sonificationPlayer.Received().Alarm();
    }
    
    [Fact]
    public void ReachEndOfTimeTimeShouldHaveNotStartedState()
    {
        StartGameAndNextPlayer();

        _scheduler.AdvanceBy(_playTime.Ticks);

        _currentChrono!.State.Should().Be(ChessChronoStates.NotStarted);
    }

    [Fact]
    public void StopGameShouldHaveNotStartedState()
    {
        StartGameAndNextPlayer();

        _timer.StopGame();

        _currentChrono!.State.Should().Be(ChessChronoStates.NotStarted);
    }
    
    private void StartGameAndNextPlayer()
    {
        _timer.StartGame(_playTime);
        _timer.NextPlayer();
    }

    public void Dispose()
    {
        _chronoSubcription.Dispose();
    }
}
