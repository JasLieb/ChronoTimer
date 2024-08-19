namespace ChronoTimer.Tests.ViewModels.Setups;

public class ChessSetupViewModelTests
{
    private readonly INavigator _navigator;
    private readonly IDeviceOrientationService _deviceOrientation;
    private readonly ChessSetupViewModel _viewModel;

    public ChessSetupViewModelTests()
    {
        _navigator = Substitute.For<INavigator>();
        _deviceOrientation = Substitute.For<IDeviceOrientationService>();
        _viewModel = new ChessSetupViewModel(_navigator, _deviceOrientation);
    }

    [Fact]
    public void OnAppearingShouldSetPortraitOrientation()
    {
        _viewModel.OnAppearing();

        _deviceOrientation.Received().SetPortrait();
    }

    [Fact]
    public void OnChangeChronoTypeCommandShouldGotoSelection()
    {
        _viewModel.ChangeChronoTypeCommand.Execute(null);

        _navigator.Received().GotoSelection();
    }
}
