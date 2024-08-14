namespace ChronoTimer.Tests.ViewModels;

public class SetupViewModelTests
{
    private readonly IChronoTimer _chronoTimer;
    private readonly INavigator _navigator;
    private readonly IDeviceOrientationService _deviceOrientation;
    private readonly SetupViewModel _viewModel;

    public SetupViewModelTests()
    {
        _chronoTimer = Substitute.For<IChronoTimer>();
        _navigator = Substitute.For<INavigator>();
        _deviceOrientation = Substitute.For<IDeviceOrientationService>();
        _viewModel = new SetupViewModel(_chronoTimer, _navigator, _deviceOrientation);
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
        _viewModel.ExerciceTime.Value = TimeSpan.FromSeconds(0);
        _viewModel.BreakTime.Value = TimeSpan.FromSeconds(1);

        _viewModel.StartExerciceCommand.Execute(null);

        _chronoTimer.DidNotReceive().StartExercice(
            Arg.Any<TimeSpan>(),
            Arg.Any<TimeSpan>()
        );
        _navigator.DidNotReceive().GotoChronoTimer();
    }
    
    [Fact]
    public void ASelectedTimeIsZeroShouldDisplayError()
    {
        _viewModel.ExerciceTime.Value = TimeSpan.FromSeconds(0);
        _viewModel.BreakTime.Value = TimeSpan.FromSeconds(1);

        _viewModel.StartExerciceCommand.Execute(null);

        _viewModel.Error.Should().Be("Time cannot be Zero");
        _viewModel.IsErrorsVisible.Should().BeTrue();
    }

    [Fact]
    public void OnStartExerciceCommandShouldStartExercice()
    {
        _viewModel.ExerciceTime.Value = TimeSpan.FromSeconds(10);
        _viewModel.BreakTime.Value = TimeSpan.FromSeconds(1);
        _viewModel.StartExerciceCommand.Execute(null);

        _chronoTimer.Received().StartExercice(
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(1)
        );
        _navigator.Received().GotoChronoTimer();
    }
}
