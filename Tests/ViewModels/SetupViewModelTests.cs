namespace ChronoTimer.Tests;

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
    public void OnStartExerciceCommandShouldStartExercice()
    {
        _viewModel.ExerciceTime = TimeSpan.FromSeconds(10);
        _viewModel.BreakTime = TimeSpan.FromSeconds(1);
        _viewModel.StartExerciceCommand.Execute(null);

        _chronoTimer.Received().StartExercice(
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(1)
        );
        _navigator.Received().GotoChronoTimer();
    }
}
