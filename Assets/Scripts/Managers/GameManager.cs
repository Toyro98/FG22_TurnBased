using System;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameState _state;

    public static event GameStateChange OnGameStateChange;
    public delegate void GameStateChange(GameState state);

    private PlayerManager _playerManager;
    
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
        _playerManager = GetComponent<PlayerManager>();   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            UpdateGameState(GameState.Start);
        }
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
            case GameState.Test:
                HandleTest();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        
        OnGameStateChange?.Invoke(_state);
    }

    private void HandleGameStart()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("<< New Game has started! Creating Players >>");
    }
    
    private void HandlePlayerTurn()
    {
        Debug.Log("<< GameState has changed to Player's Turn >>");
    }

    private void HandleTest()
    {

    }

    private void HandleGameOver()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
     
        Debug.Log("<< Game Over >>");
    }
}

public enum GameState
{
    Start,
    PlayerTurn,
    Test,
    GameOver
}