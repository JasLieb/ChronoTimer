using System.Reactive.Subjects;
using ChronoTimer.Core.Models;
using ChronoTimer.Core.Models.ChronoStates;
using ChronoTimer.Core.Providers;
using CommunityToolkit.Maui.Extensions;

namespace ChronoTimer.Maui.Controls;

public partial class ChronoTimer : ContentView
{
    private BehaviorSubject<Color> _currentColorSubject = new(Colors.Transparent);
    public IObservable<Color> CurrentColorObservable => _currentColorSubject;

    public static readonly BindableProperty ChronoStateProperty =
        BindableProperty.Create(
            nameof(ChronoState),
            typeof(ExerciceChronoState),
            typeof(ChronoTimer),
            new ExerciceChronoState(),
            propertyChanged: (bindable, oldValue, newValue) =>
                (bindable as ChronoTimer)?.OnChronoStateUpdate(
                    oldValue as ExerciceChronoState,
                    newValue as ExerciceChronoState
                )
        );

    public ExerciceChronoState ChronoState
    {
        get => (ExerciceChronoState)GetValue(ChronoStateProperty);
        set => SetValue(ChronoStateProperty, value);
    }

    public ChronoTimer()
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

        EllipsePulse.ChronoState = newState;
        RemainingTimeLabel.Text =
            $"{newState.RemainingTime:mm}:{newState.RemainingTime:ss}";

        var newColor = ConvertRgbToColor(
            ColorProvider.GetStateColor(newState)
        );

        if (_currentColorSubject.Value != newColor)
            Task.Run(
                async () => await UpdateContainerBackgroundColor(newColor)
            );
    }

    private async Task UpdateContainerBackgroundColor(Color newColor)
    {
        _currentColorSubject.OnNext(newColor);

        await ChronoTimerContainer.BackgroundColorTo(
            newColor,
            easing: Easing.Linear
        );
    }

    private static Color ConvertRgbToColor(RGB? newRgbColor) => 
        newRgbColor is RGB rgb
        ? Color.FromRgb(rgb.Red, rgb.Green, rgb.Blue)
        : Colors.Transparent;
}
