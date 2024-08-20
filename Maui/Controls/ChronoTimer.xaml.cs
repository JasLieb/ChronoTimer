using System.Reactive.Subjects;
using System.Windows.Input;
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
            typeof(GenericChronoState),
            typeof(ChronoTimer),
            null,
            propertyChanged: (bindable, _, newValue) =>
                (bindable as ChronoTimer)?.OnChronoStateUpdate(
                    newValue as GenericChronoState
                )
        );

    public GenericChronoState? ChronoState
    {
        get => GetValue(ChronoStateProperty) as GenericChronoState;
        set => SetValue(ChronoStateProperty, value);
    }

    public static readonly BindableProperty IsRevertedProperty =
        BindableProperty.Create(
            nameof(IsReverted),
            typeof(bool),
            typeof(ChronoTimer),
            false,
            propertyChanged: async (bindable, _, newValue) =>
            {
                if (
                    bindable is ChronoTimer chronoTimer
                    && newValue is bool mustRevert
                    && mustRevert
                )
                    await chronoTimer.Revert();
            }
        );

    public bool IsReverted
    {
        get => (bool)GetValue(IsRevertedProperty);
        set => SetValue(IsRevertedProperty, value);
    }

    public static readonly BindableProperty IsEllipsesVisibleProperty =
        BindableProperty.Create(
            nameof(IsEllipsesVisible),
            typeof(bool),
            typeof(ChronoTimer),
            false,
            propertyChanged: (bindable, _, newValue) =>
            {
                if (
                    bindable is ChronoTimer chronoTimer
                    && newValue is bool isVisible
                )
                    chronoTimer.DisplayEllipses(isVisible);
            }
        );

    public bool IsEllipsesVisible
    {
        get => (bool)GetValue(IsEllipsesVisibleProperty);
        set => SetValue(IsEllipsesVisibleProperty, value);
    }

    public static readonly BindableProperty OnTapCommandProperty =
        BindableProperty.Create(
            nameof(OnTapCommand),
            typeof(ICommand),
            typeof(ChronoTimer),
            null
        );

    public ICommand? OnTapCommand
    {
        get => GetValue(OnTapCommandProperty) as ICommand;
        set => SetValue(OnTapCommandProperty, value);
    }

    public ChronoTimer()
    {
        InitializeComponent();
    }

    protected void OnTapped(object sender, EventArgs args) =>
        OnTapCommand?.Execute(null);

    private async Task Revert()
    {
        await ChronoTimerContainer.RotateXTo(180, length: 0);
        await ChronoTimerContainer.RotateYTo(180, length: 0);
    }

    private void DisplayEllipses(bool isVisible) =>
        EllipsePulse.IsVisible = isVisible;

    private void OnChronoStateUpdate(
        GenericChronoState? newState
    )
    {
        newState ??= new();

        if (IsEllipsesVisible)
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
