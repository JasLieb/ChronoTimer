﻿namespace ChronoTimer.Tests;

public class ChronoTimerServiceTests : IDisposable
{
    
    private readonly TimeSpan _exerciceTime = TimeSpan.FromSeconds(3);
    private readonly TimeSpan _breakTime = TimeSpan.FromSeconds(1);

    private readonly ChronoTimerService _timer;
    private readonly TestScheduler _scheduler = new TestScheduler();
    private readonly ISonificationPlayer _sonificationPlayer = Substitute.For<ISonificationPlayer>();
    private readonly IDisposable _chronoSubcription;
    private ChronoState? _currentChrono;

    public ChronoTimerServiceTests()
    {
        _timer = new ChronoTimerService(_scheduler, _sonificationPlayer);
        _chronoSubcription = _timer.Chrono.Subscribe(
            chrono => _currentChrono = chrono
        );
    }

    [Fact]
    public void NotStartedTimerShouldHaveNotStartedState()
    {
        _currentChrono!.State.Should().Be(ChronoStates.NotStarted);
        _currentChrono.RemainingTime.Should().BeNull();
        _currentChrono.OriginalTime.Should().BeNull();
    }

    [Fact]
    public void StopExerciceShouldHaveNotStartedState()
    {
        StartExercice();

        _timer.StopExercice();        
    
        _currentChrono!.State.Should().Be(ChronoStates.NotStarted);
        _currentChrono.RemainingTime.Should().BeNull();
        _currentChrono.OriginalTime.Should().BeNull();
    }

    [Fact]
    public void StartExerciceShouldUpdateChronoWithExerciceTime()
    {
        StartExercice();

        _currentChrono!.State.Should().Be(ChronoStates.ExerciceTime);
        _currentChrono.RemainingTime.Should().Be(_exerciceTime);
        _currentChrono.OriginalTime.Should().Be(_exerciceTime);
    }

    [Fact]
    public void RunExerciceShouldUpdateChronoWithExerciceTime()
    {
        StartExercice();
        var elapsedTime = TimeSpan.FromSeconds(1);

        _scheduler.AdvanceBy(elapsedTime.Ticks);

        _currentChrono!.State.Should().Be(ChronoStates.ExerciceTime);
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

        _currentChrono!.State.Should().Be(ChronoStates.BreakTime);
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

        _currentChrono!.State.Should().Be(ChronoStates.ExerciceTime);
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
