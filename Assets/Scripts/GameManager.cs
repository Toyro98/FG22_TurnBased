using System;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameState _state;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Start);
    }

    public void UpdateGameState(GameState state)
    {
        _state = state;
        
        switch (_state)
        {
            case GameState.Start:
                HandleGameStart();
                break;
            case GameState.PlayerTurn:
                HandlePlayerTurn();
                break;
            case GameState.RandomDrop:
                HandleRandomDrop();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void HandleGameStart()
    {
        // TODO
    }
    
    private void HandlePlayerTurn()
    {
        // TODO
    }

    private void HandleRandomDrop()
    {
        // TODO
    }

    private void HandleGameOver()
    {
        // TODO
    }
}

public enum GameState
{
    Start,
    PlayerTurn,
    RandomDrop,
    GameOver
}