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
			typeof(GenericChronoState), 
			typeof(EllipsePulse),
			null,
			propertyChanged: (bindable, oldValue, newValue) =>
				(bindable as EllipsePulse)?.OnChronoStateUpdate(
					oldValue as GenericChronoState, 
					newValue as GenericChronoState
				)
		);
	
	public GenericChronoState? ChronoState
	{
		get => GetValue(ChronoStateProperty) as GenericChronoState;
		set => SetValue(ChronoStateProperty, value);
	}

	public EllipsePulse()
	{
		InitializeComponent();
	}

	private void OnChronoStateUpdate(
		GenericChronoState? oldState, 
		GenericChronoState? newState
	)
    {
        UpdateEllipseVisibility(newState);

        if( newState is not null)
        {
            InitAnimationLengthStepOnStateChanged(oldState, newState);

            var oldAnimationLength = _currentAnimationStepLength;
            
            UpdateAnimationLengthStep(newState);

            if (
                oldState?.IsUrgent != newState.IsUrgent
                || oldAnimationLength != _currentAnimationStepLength
            )
            AnimateEllipse(newState);
        }
    }

    private void InitAnimationLengthStepOnStateChanged(
        GenericChronoState? oldState, 
        GenericChronoState newState
    )
    {
        var oldIsUrgent = oldState?.IsUrgent ?? false;
        var newIsUrgent = newState.IsUrgent ?? false;
        var mustSwitchToUrgentMode = !oldIsUrgent && newIsUrgent;

        if (mustSwitchToUrgentMode)
        {
            _animationLengthSteps = [
                (newState.OriginalTime.Ticks * 0.75, 2500),
                (newState.OriginalTime.Ticks * 0.5, 2000),
                (newState.OriginalTime.Ticks * 0.25, 1500),
                (0, 1000)
            ];
        }

        var mustSwitchToNotUrgentMode = oldIsUrgent && !newIsUrgent;

        if (mustSwitchToNotUrgentMode)
        {
            _animationLengthSteps = [
                (newState.OriginalTime.Ticks * 0.75, 1500),
                (newState.OriginalTime.Ticks * 0.5, 2000),
                (newState.OriginalTime.Ticks * 0.25, 2500),
                (0, 3000)
            ];
        }
    }

    private void UpdateAnimationLengthStep(GenericChronoState newState) =>
        _currentAnimationStepLength = 
            _animationLengthSteps.FirstOrDefault(
                animationStep => newState.RemainingTime.Ticks >= animationStep.Step
            )
            .Length;

    private void UpdateEllipseVisibility(GenericChronoState? chronoState)
    {
        firstEllipse.IsVisible = chronoState is not null;
        secondEllipse.IsVisible = chronoState?.IsUrgent ?? false;
        thirdEllipse.IsVisible = chronoState?.IsUrgent ?? false;
    }

	private void AnimateEllipse(GenericChronoState chronoState)
    {
        AbortAnimation();

        if (chronoState.IsUrgent is not null)
        {
            var baseTime = (bool)chronoState.IsUrgent
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
    }

    private void AbortAnimation()
    {
        container.AbortAnimation(AninationName);
        UpdateEllipseVisibility(null);
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
