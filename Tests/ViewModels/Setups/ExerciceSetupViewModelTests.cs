namespace ChronoTimer.Tests.ViewModels.Setups;

public class ExerciceSetupViewModelTests
{
    private readonly INavigator _navigator;
    private readonly IDeviceOrientationService _deviceOrientation;
    private readonly ExerciceSetupViewModel _viewModel;

    public ExerciceSetupViewModelTests()
    {
        _navigator = Substitute.For<INavigator>();
        _deviceOrientation = Substitute.For<IDeviceOrientationService>();
        _viewModel = new ExerciceSetupViewModel(_navigator, _deviceOrientation);
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

        _navigator.DidNotReceive().GotoChronoTimer(Arg.Any<ExerciceRequest>());
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
        var exerciceTime = TimeSpan.FromSeconds(10);
        _viewModel.ExerciceTime.Value = exerciceTime;
        var breakTime = TimeSpan.FromSeconds(1);
        _viewModel.BreakTime.Value = breakTime;
        
        _viewModel.StartExerciceCommand.Execute(null);
        
        _navigator.Received().GotoChronoTimer(
            Arg.Is<ExerciceRequest>(
                request => 
                    request.ExerciceTime == exerciceTime
                    && request.BreakTime == breakTime
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
