using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class UIManger : MonoBehaviour
{
    readonly WaitForSeconds delay = new WaitForSeconds(1f);
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _gameoverScreen;

    [SerializeField] private GameObject _gameTimerScreen;
    [SerializeField] private TMP_Text _gameTimer;
    [SerializeField] private GameObject _playerTimerScreen;
    [SerializeField] private TMP_Text _playerTimer;
    [SerializeField] private GameObject _playerHealthScreen;
    [SerializeField] private TMP_Text _playerHealth;
    [SerializeField] private GameObject _playerNameScreen;
    [SerializeField] private TMP_Text _playerName;

    [SerializeField] private GameObject _playerChargeScreen;
    [SerializeField] private Slider _playerCharge;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        GameManager.OnGameStateChange += GameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= GameStateChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_gameoverScreen.activeInHierarchy)
        {
            if (GameManager.IsGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        } 
    }

    public void EnablePlayerChargeUI()
    {
        _playerChargeScreen.SetActive(true);
    }

    public void UpdatePlayerCharge(float value)
    {
        _playerCharge.value = value;
    }

    public void ResumeGame()
    {
        _pauseScreen.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        GameManager.IsGamePaused = false;
    }

    public void PauseGame()
    {
        _pauseScreen.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        GameManager.IsGamePaused = true;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.PlayerSwitch)
        {
            return;
        }

        _playerNameScreen.SetActive(state == GameState.PlayerTurn);
        _playerHealthScreen.SetActive(state == GameState.PlayerTurn);
        _playerTimerScreen.SetActive(state == GameState.PlayerTurn);
        _playerChargeScreen.SetActive(false);
        _crosshair.SetActive(state == GameState.PlayerTurn);
        _gameoverScreen.SetActive(state == GameState.GameOver);

        if (state == GameState.Start)
        {
            _gameTimerScreen.SetActive(true);
            StartCoroutine(StartGameTimer());
        }
        else if (state == GameState.PlayerTurn)
        {
            _playerName.text = _playerManager.GetActivePlayerName();
            _playerHealth.text = _playerManager.GetActivePlayerHealth();
            _coroutine = StartCoroutine(StartPlayerTimer());
        }
        else if (state == GameState.GameOver)
        {
            StopAllCoroutines();
        }
        else if (state == GameState.Wait)
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator StartGameTimer()
    {
        int time = GameManager.Instance.GameSettings.gameTime * 60;

        // Everything is now prepared and is now ready to update the game state
        GameManager.Instance.UpdateGameState(GameState.PlayerTurn);

        while (time > 0)
        {
            var timeSpan = TimeSpan.FromSeconds(time--);

            if (time >= 3600)
            {
                _gameTimer.text = string.Format("{0:D}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }
            else
            {
                _gameTimer.text = string.Format("{0:D}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            }
            
            yield return delay;
        }

        // Time ran out 
        GameManager.Instance.UpdateGameState(GameState.GameOver);
    }

    private IEnumerator StartPlayerTimer()
    {
        int time = GameManager.Instance.GameSettings.timePerPlayer;
        _playerTimerScreen.SetActive(true);

        while (time > 0)
        {
            var timeSpan = TimeSpan.FromSeconds(time--);
            _playerTimer.text = string.Format("{0:D2}", timeSpan.Seconds);

            yield return delay;
        }

        // Hide the timer if the time ran out and switch player
        _playerTimerScreen.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.PlayerSwitch);
    }
}
