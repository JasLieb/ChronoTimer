﻿using ChronoTimer.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChronoTimer.Core.Models.Requests;
using ChronoTimer.Core.Models.ChronoStates;
using ChronoTimer.Core.Services.ChronoTimer.Chess;
using System.Reactive.Linq;

namespace ChronoTimer.Core.ViewModels.ChronoTimer;

public partial class ChessChronoTimerViewModel : ObservableObject, IDisposable, IQueryAttributable
{
    private readonly IDisposable _chronoStateSubscription;
    private readonly IChessChronoTimer _chronoTimer;
    private readonly INavigator _navigator;
    private readonly IDeviceOrientationService _deviceOrientation;
    
    [ObservableProperty]
    private ChessChronoState _chronoState = new();
    
    [ObservableProperty]
    private bool _isFirstPlayerEnabled = false;
    
    [ObservableProperty]
    private bool _isSecondPlayerEnabled = false;

    public ChessChronoTimerViewModel(
        IChessChronoTimer chronoTimer,
        INavigator navigator,
        IDeviceOrientationService deviceOrientation
    )
    {
        _chronoTimer = chronoTimer;
        _navigator = navigator;
        _deviceOrientation = deviceOrientation;
        _chronoStateSubscription =
            chronoTimer.Chrono
                .Delay(TimeSpan.FromMilliseconds(500))
                .Subscribe(UpdateViewModelState);
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query) 
    {
        _deviceOrientation.SetLandscape();
        if (
            query.TryGetValue("request", out var value) 
            && value is ChessRequest request
        )
        {
            _chronoTimer.StartGame(request.TimePerPlayer);
        }
    }

    [RelayCommand]
    public void NextPlayer() =>
        _chronoTimer.NextPlayer();

    [RelayCommand]
    private void GotoChessSetupPage()
    {
        _chronoTimer.StopGame();
        _navigator.GotoChessSetup();
    }

    private void UpdateViewModelState(ChessChronoState chrono)
    {
        ChronoState = chrono;
        IsFirstPlayerEnabled = chrono.State is ChessChronoStates.FirstPlayerTime;
        IsSecondPlayerEnabled = 
            chrono.State is ChessChronoStates.SecondPlayerTime 
            or ChessChronoStates.AwaitPlayerStartGame;
    }

    public void Dispose() =>
        _chronoStateSubscription.Dispose();
}
