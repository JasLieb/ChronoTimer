using ChronoTimer.Core.Models.ChronoStates;

namespace ChronoTimer.Tests.Services.ChronoTimers;

public class ExerciceChronoTimerServiceTests : IDisposable
{

    private readonly TimeSpan _exerciceTime = TimeSpan.FromSeconds(3);
    private readonly TimeSpan _breakTime = TimeSpan.FromSeconds(1);

    private readonly ExerciceChronoTimerService _timer;
    private readonly TestScheduler _scheduler = new();
    private readonly ISonificationPlayer _sonificationPlayer = Substitute.For<ISonificationPlayer>();
    private readonly IDisposable _chronoSubcription;
    private ExerciceChronoState? _currentChrono;

    public ExerciceChronoTimerServiceTests()
    {
        _timer = new ExerciceChronoTimerService(_scheduler, _sonificationPlayer);
        _chronoSubcription = _timer.Chrono.Subscribe(
            chrono => _currentChrono = chrono
        );
    }

    [Fact]
    public void NotStartedTimerShouldHaveNotStartedState()
    {
        _currentChrono!.State.Should().Be(ExerciceChronoStates.NotStarted);
    }

    [Fact]
    public void StopExerciceShouldHaveNotStartedState()
    {
        StartExercice();

        _timer.StopExercice();

        _currentChrono!.State.Should().Be(ExerciceChronoStates.NotStarted);
    }

    [Fact]
    public void StartExerciceShouldUpdateChronoWithExerciceTime()
    {
        StartExercice();

        _currentChrono!.State.Should().Be(ExerciceChronoStates.ExerciceTime);
        _currentChrono.RemainingTime.Should().Be(_exerciceTime);
        _currentChrono.OriginalTime.Should().Be(_exerciceTime);
    }

    [Fact]
    public void RunExerciceShouldUpdateChronoWithExerciceTime()
    {
        StartExercice();
        var elapsedTime = TimeSpan.FromSeconds(1);

        _scheduler.AdvanceBy(elapsedTime.Ticks);

        _currentChrono!.State.Should().Be(ExerciceChronoStates.ExerciceTime);
        _currentChrono.OriginalTime.Should().Be(_exerciceTime);
        _currentChrono.RemainingTime.Should().Be(
            _exerciceTime - elapsedTime
        );
    }

    [Fact]
    public void CompleteExerciceTimeShouldPlaySonification()
    {
        StartExercice();

        _scheduler.AdvanceBy(_exerciceTime.Ticks);

        _sonificationPlayer.Received().Alarm();
    }

    [Fact]
    public void CompleteExerciceShouldUpdateChronoWithBreakTime()
    {
        StartExercice();

        _scheduler.AdvanceTo(_exerciceTime.Ticks);

        _currentChrono!.State.Should().Be(ExerciceChronoStates.BreakTime);
        _currentChrono.RemainingTime.Should().Be(_breakTime);
        _currentChrono.OriginalTime.Should().Be(_breakTime);
    }

    [Fact]
    public void CompleteBreakShouldUpdateChronoWithExerciceTime()
    {
        StartExercice();

        _scheduler.AdvanceTo(
            (_exerciceTime + _breakTime).Ticks
        );

        _currentChrono!.State.Should().Be(ExerciceChronoStates.ExerciceTime);
        _currentChrono.RemainingTime.Should().Be(_exerciceTime);
        _currentChrono.OriginalTime.Should().Be(_exerciceTime);
    }

    [Fact]
    public void CompleteBreakTimeShouldPlaySonification()
    {
        StartExercice();
        _scheduler.AdvanceBy(_exerciceTime.Ticks);
        _sonificationPlayer.ClearReceivedCalls();

        _scheduler.AdvanceBy(_breakTime.Ticks);

        _sonificationPlayer.Received().Alarm();
    }

    private void StartExercice() =>
        _timer.StartExercice(_exerciceTime, _breakTime);


    public void Dispose()
    {
        _chronoSubcription.Dispose();
    }
}
