using System;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameState _state;
    
    public static event GameStateChange OnGameStateChange;
    public delegate void GameStateChange(GameState state);

    // Managers
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private UIManger _uiManger;
    
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
        _uiManger = GetComponent<UIManger>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
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
        
        OnGameStateChange?.Invoke(_state);
    }

    private void HandleGameStart()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _playerManager.CreatePlayers(2);
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
        _playerManager.DisableAllPlayerInputs();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

public enum GameState
{
    Start,
    PlayerTurn,
    RandomDrop,
    GameOver
}