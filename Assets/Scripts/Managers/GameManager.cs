using System;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event GameStateChange OnGameStateChange;
    public delegate void GameStateChange(GameState state);

    public GameSettings GameSettings { get; private set; }
   
    [SerializeField] private PlayerManager _playerManager;

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
        GameSettings = FindObjectOfType<GameSettings>();

        // Remove this before leaving in assignment
        // Only here if you didn't start from mainmenu
        if (GameSettings == null)
        {
            GameSettings = gameObject.AddComponent<GameSettings>();
        }

        UpdateGameState(GameState.Start);
    }

    public void UpdateGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                HandleGameStart();
                break;
            case GameState.PlayerTurn:
                HandlePlayerTurn();
                break;
            case GameState.PlayerSwitch:
                HandlePlayerSwitch();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        
        OnGameStateChange?.Invoke(state);
    }

    private void HandleGameStart()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("<< New Game has started! Creating Players >>");
        _playerManager.CreatePlayers();
    }
    
    private void HandlePlayerTurn()
    {
        Debug.Log("<< GameState has changed to Player's Turn >>");
    }

    private void HandlePlayerSwitch()
    {
        Debug.Log("<< Switching Player >>");
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
    PlayerSwitch,
    GameOver
}