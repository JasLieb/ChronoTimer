using ChronoTimer.Core.Models;

namespace ChronoTimer.Maui.Controls;

public partial class EllipsePulse : ContentView
{
	private const string AninationName = "EllipseAnimation";

	public static readonly BindableProperty ChronoStateProperty = 
		BindableProperty.Create(
			nameof(ChronoState), 
			typeof(ChronoState), 
			typeof(EllipsePulse),
			new ChronoState(),
			propertyChanged: (bindable, oldValue, newValue) =>
				(bindable as EllipsePulse)?.OnChronoStateUpdate(
					oldValue as ChronoState, 
					newValue as ChronoState
				)
		);
	
	public ChronoState ChronoState
	{
		get => (ChronoState)GetValue(ChronoStateProperty);
		set => SetValue(ChronoStateProperty, value);
	}

	public EllipsePulse()
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

		if(oldState.State != newState.State)
			AnimateEllipse(newState);

        UpdateEllipseVisibility(newState);
    }

    private void UpdateEllipseVisibility(ChronoState chronoState)
    {
        firstEllipse.IsVisible = chronoState.State is not ChronoStates.NotStarted;
        secondEllipse.IsVisible = chronoState.State is ChronoStates.ExerciceTime;
        thirdEllipse.IsVisible = chronoState.State is ChronoStates.ExerciceTime;
    }

	private void AnimateEllipse(ChronoState newState)
    {
		container.AbortAnimation(AninationName);

		var baseTime = newState.State is ChronoStates.ExerciceTime 
            ? (uint)2000 
            : 3000;

		container.Animate(
			AninationName,
			MakeEllipseAnimation(),
			length: baseTime,
			easing: Easing.Linear,
			repeat: () => true
		);
    }

	private Animation MakeEllipseAnimation()
	{
		var scaleInAnimation = new Animation(v => container.Scale = v, start: 0, end: 10, easing: Easing.Linear);
		var fadeOutAnimation = new Animation(v => container.Opacity = v, start: 1, end: 0, easing: Easing.Linear);

		return new Animation()
		{
			{0, 1, scaleInAnimation},
			{0, 1, fadeOutAnimation}
		};
	}
}
