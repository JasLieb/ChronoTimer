using System.Reactive.Subjects;
using ChronoTimer.Core.Models;
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
