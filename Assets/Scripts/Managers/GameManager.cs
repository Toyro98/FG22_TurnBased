using System;
using System.Collections;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool IsGamePaused = false;

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
        // Get GameSettings from the gameobject that's always loaded
        GameSettings = FindObjectOfType<GameSettings>();

        // Once we got the gamesettings, prepare to start the game
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
                Debug.Log("<< GameState has changed to Player's Turn >>");
                break;
            case GameState.PlayerSwitch:
                Debug.Log("<< Switching Player >>");
                break;
            case GameState.Wait:
                Debug.Log("<< Waiting >>");
                StartCoroutine(Wait());
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

    private void HandleGameOver()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
     
        Debug.Log("<< Game Over >>");
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);

        _playerManager.CheckIfPlayersAreAlive();
    }
}

public enum GameState
{
    Start,
    PlayerTurn,
    PlayerSwitch,
    Wait,
    GameOver
}