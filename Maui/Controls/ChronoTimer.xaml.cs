using ChronoTimer.Core.Models;
using ChronoTimer.Core.Providers;
using CommunityToolkit.Maui.Extensions;

namespace ChronoTimer.Maui.Controls;

public partial class ChronoTimer : ContentView
{
    public static readonly BindableProperty ChronoStateProperty = 
		BindableProperty.Create(
			nameof(ChronoState), 
			typeof(ChronoState), 
			typeof(ChronoTimer),
			new ChronoState(),
			propertyChanged: (bindable, oldValue, newValue) =>
				(bindable as ChronoTimer)?.OnChronoStateUpdate(
					oldValue as ChronoState, 
					newValue as ChronoState
				)
		);
	
	public ChronoState ChronoState
	{
		get => (ChronoState)GetValue(ChronoStateProperty);
		set => SetValue(ChronoStateProperty, value);
	}

	public ChronoTimer()
	{
		InitializeComponent();
	}

    private void OnChronoStateUpdate(
		ChronoState? oldState, 
		ChronoState? newState
	)
    {
        oldState ??= new();
        newState ??= new();

		EllipsePulse.ChronoState = newState;
		RemainingTimeLabel.Text = 
            $"{newState.RemainingTime:mm}:{newState.RemainingTime:ss}";

        var oldColor = ColorProvider.GetStateColor(oldState);
        var newColor = ColorProvider.GetStateColor(newState);

        if(oldColor != newColor)
            Task.Run(
                async () => 
                    await ChronoTimerContainer.BackgroundColorTo(
                        newColor is RGB rgb
                        ? Color.FromRgb(rgb.Red, rgb.Green, rgb.Blue)
                        : Colors.Transparent,
                        easing: Easing.Linear
                    )
            );
    }
}
