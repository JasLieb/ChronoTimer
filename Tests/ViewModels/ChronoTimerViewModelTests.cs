using ChronoTimer.Core.Models.Requests;

namespace ChronoTimer.Tests.ViewModels;

public class ChronoTimerViewModelTests
{
    private readonly Subject<ChronoState> _chronoStateSubject = new();
    private readonly IChronoTimer _chronoTimer;
    private readonly INavigator _navigator;
    private readonly IDeviceOrientationService _deviceOrientation;
    private readonly ChronoTimerViewModel _viewModel;

    public ChronoTimerViewModelTests()
    {
        _chronoTimer = Substitute.For<IChronoTimer>();
        _navigator = Substitute.For<INavigator>();
        _chronoTimer.Chrono.Returns(_chronoStateSubject);
        _deviceOrientation = Substitute.For<IDeviceOrientationService>();
        _viewModel = new ChronoTimerViewModel(_chronoTimer, _navigator, _deviceOrientation);
    }

    [Fact]
    public void OnApplyQueryAttributesShouldSetLandscapeOrientation()
    {
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object>());

        _deviceOrientation.Received().SetLandscape();
    }
    
    [Fact]
    public void OnApplyQueryAttributesShouldStartExercice()
    {
        var exerciceTime = TimeSpan.FromSeconds(10);
        var breakTime = TimeSpan.FromSeconds(1);
        var exerciceArgs = new Dictionary<string, object>()
        {
            { "request", new ExerciceRequest(exerciceTime, breakTime)},
        };

        _viewModel.ApplyQueryAttributes(exerciceArgs);

        _chronoTimer.Received().StartExercice(
            exerciceTime,
            breakTime
        );
    }

    [Fact]
    public void DisplayedRemainingTimeShouldBeActualRemainingTime()
    {
        _chronoStateSubject.OnNext(
            new(ChronoStates.ExerciceTime, TimeSpan.FromSeconds(1))
        );

        _viewModel.ChronoState.State.Should().Be(ChronoStates.ExerciceTime);
        _viewModel.ChronoState.RemainingTime.Should().Be(TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ExecuteGotoExerciceSetupPageCommandShouldGStopTimer()
    {
        _viewModel.GotoExerciceSetupPageCommand.Execute(null);
        _chronoTimer.Received().StopExercice();
    }

    [Fact]
    public void ExecuteGotoExerciceSetupPageCommandShouldGotoExerciceSetup()
    {
        _viewModel.GotoExerciceSetupPageCommand.Execute(null);
        _navigator.Received().GotoExerciceSetup();
    }
}
