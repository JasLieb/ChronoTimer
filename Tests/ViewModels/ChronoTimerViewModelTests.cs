using System.Reactive.Subjects;

namespace ChronoTimer.Tests;

public class ChronoTimerViewModelTests
{
    private readonly Subject<ChronoState> _chronoStateSubject = new();
    private readonly IChronoTimer _chronoTimer;
    private readonly INavigator _navigator;
    private readonly ChronoTimerViewModel _viewModel;

    public ChronoTimerViewModelTests()
    {
        _chronoTimer = Substitute.For<IChronoTimer>();
        _navigator = Substitute.For<INavigator>();
        _chronoTimer.Chrono.Returns(_chronoStateSubject);
        _viewModel = new ChronoTimerViewModel(_chronoTimer, _navigator);
    }

    [Fact]
    public void DisplayedRemainingTimeShouldBeActualRemainingTime()
    {
        _chronoStateSubject.OnNext(
            new (ChronoStates.ExerciceTime, TimeSpan.FromSeconds(1))
        );

        _viewModel.ChronoState.State.Should().Be(ChronoStates.ExerciceTime);
        _viewModel.ChronoState.RemainingTime.Should().Be(TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void NotStartedShouldDisplayGotoSetUpButton()
    {
        _chronoStateSubject.OnNext(new());
        _viewModel.TimerColor.Should().BeNull();
    }

    [Fact]
    public void ExerciceTimeShouldHaveRedColor()
    {
        _chronoStateSubject.OnNext(new(ChronoStates.ExerciceTime, TimeSpan.MaxValue));        
        _viewModel.TimerColor.Should().BeEquivalentTo(new RGB(254, 27, 0));
    }

    [Fact]
    public void BreakTimeShouldHaveGreenColor()
    {
        _chronoStateSubject.OnNext(new(ChronoStates.BreakTime, TimeSpan.MaxValue));
        _viewModel.TimerColor.Should().BeEquivalentTo(new RGB(0,128,0));
    }

    [Fact]
    public void ExecuteGotoSetupPageCommandShouldGStopTimer()
    {
        _viewModel.GotoSetupPageCommand.Execute(null);
        _chronoTimer.Received().StopExercice();
    }

    [Fact]
    public void ExecuteGotoSetupPageCommandShouldGotoSetup()
    {
        _viewModel.GotoSetupPageCommand.Execute(null);
        _navigator.Received().GotoSetup();
    }
}
