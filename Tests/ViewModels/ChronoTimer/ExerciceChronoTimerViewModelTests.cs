namespace ChronoTimer.Tests.ViewModels.ChronoTimer;

public class ExerciceChronoTimerViewModelTests
{
    private readonly Subject<ExerciceChronoState> _chronoStateSubject = new();
    private readonly IExerciceChronoTimer _chronoTimer;
    private readonly INavigator _navigator;
    private readonly IDeviceOrientationService _deviceOrientation;
    private readonly ExerciceChronoTimerViewModel _viewModel;

    public ExerciceChronoTimerViewModelTests()
    {
        _chronoTimer = Substitute.For<IExerciceChronoTimer>();
        _navigator = Substitute.For<INavigator>();
        _chronoTimer.Chrono.Returns(_chronoStateSubject);
        _deviceOrientation = Substitute.For<IDeviceOrientationService>();
        _viewModel = new ExerciceChronoTimerViewModel(_chronoTimer, _navigator, _deviceOrientation);
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
            new(ExerciceChronoStates.ExerciceTime, TimeSpan.FromSeconds(1))
        );

        _viewModel.ChronoState.State.Should().Be(ExerciceChronoStates.ExerciceTime);
        _viewModel.ChronoState.RemainingTime.Should().Be(TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ExecuteGotoExerciceSetupPageCommandShouldStopTimer()
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
