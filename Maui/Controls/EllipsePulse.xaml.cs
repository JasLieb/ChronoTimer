using ChronoTimer.Core.Models.ChronoStates;

namespace ChronoTimer.Maui.Controls;

public partial class EllipsePulse : ContentView
{
	private const string AninationName = "EllipseAnimation";

    private (double Step, uint Length)[] _animationLengthSteps = [];
    private uint _currentAnimationStepLength = 0;

	public static readonly BindableProperty ChronoStateProperty = 
		BindableProperty.Create(
			nameof(ChronoState), 
			typeof(ExerciceChronoState), 
			typeof(EllipsePulse),
			new ExerciceChronoState(),
			propertyChanged: (bindable, oldValue, newValue) =>
				(bindable as EllipsePulse)?.OnChronoStateUpdate(
					oldValue as ExerciceChronoState, 
					newValue as ExerciceChronoState
				)
		);
	
	public ExerciceChronoState ChronoState
	{
		get => (ExerciceChronoState)GetValue(ChronoStateProperty);
		set => SetValue(ChronoStateProperty, value);
	}

	public EllipsePulse()
	{
		InitializeComponent();
	}

	private void OnChronoStateUpdate(
		ExerciceChronoState? oldState, 
		ExerciceChronoState? newState
	)
    {
        oldState ??= new();
        newState ??= new();
        
        UpdateEllipseVisibility(newState);

        InitAnimatioLengthStepOnStateChanged(oldState, newState);

        var oldAnimationLength = _currentAnimationStepLength;
        
        UpdateAnimatioLengthStep(newState);

        if (
            oldState.State != newState.State
            || oldAnimationLength != _currentAnimationStepLength
        )
            AnimateEllipse(newState);
    }

    private void InitAnimatioLengthStepOnStateChanged(ExerciceChronoState oldState, ExerciceChronoState newState)
    {
        var isExerciceStarted =
            oldState.State is not ExerciceChronoStates.ExerciceTime
            && newState.State is ExerciceChronoStates.ExerciceTime;

        if (isExerciceStarted)
        {
            _animationLengthSteps = [
                (newState.OriginalTime.Ticks * 0.75, 2500),
                (newState.OriginalTime.Ticks * 0.5, 2000),
                (newState.OriginalTime.Ticks * 0.25, 1500),
                (0, 1000)
            ];
        }

        var isBreakTimeStarted =
            oldState.State is not ExerciceChronoStates.BreakTime
            && newState.State is ExerciceChronoStates.BreakTime;

        if (isBreakTimeStarted)
        {
            _animationLengthSteps = [
                (newState.OriginalTime.Ticks * 0.75, 1500),
                (newState.OriginalTime.Ticks * 0.5, 2000),
                (newState.OriginalTime.Ticks * 0.25, 2500),
                (0, 3000)
            ];
        }
    }

    private void UpdateAnimatioLengthStep(ExerciceChronoState newState) =>
        _currentAnimationStepLength = 
            _animationLengthSteps.FirstOrDefault(
                animationStep => newState.RemainingTime.Ticks >= animationStep.Step
            )
            .Length;

    private void UpdateEllipseVisibility(ExerciceChronoState chronoState)
    {
        firstEllipse.IsVisible = chronoState.State is not ExerciceChronoStates.NotStarted;
        secondEllipse.IsVisible = chronoState.State is ExerciceChronoStates.ExerciceTime;
        thirdEllipse.IsVisible = chronoState.State is ExerciceChronoStates.ExerciceTime;
    }

	private void AnimateEllipse(ExerciceChronoState chronoState)
    {
		container.AbortAnimation(AninationName);

        if(chronoState.State is ExerciceChronoStates.NotStarted)
            return;

		var baseTime = chronoState.State is ExerciceChronoStates.ExerciceTime 
            ? _currentAnimationStepLength
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
