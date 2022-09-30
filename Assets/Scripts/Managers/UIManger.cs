using System;
using System.Collections;
using UnityEngine;
using TMPro;

public sealed class UIManger : MonoBehaviour
{
    readonly WaitForSeconds delay = new WaitForSeconds(1f);
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private GameObject _gameoverScreen;
    [SerializeField] private GameObject _gameTimerScreen;
    [SerializeField] private TMP_Text _gameTimer;
    [SerializeField] private GameObject _playerTimerScreen;
    [SerializeField] private TMP_Text _playerTimer;
    [SerializeField] private GameObject _playerHealthScreen;
    [SerializeField] private TMP_Text _playerHealth;
    [SerializeField] private GameObject _playerNameScreen;
    [SerializeField] private TMP_Text _playerName;

    private void OnEnable()
    {
        GameManager.OnGameStateChange += GameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= GameStateChanged;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.PlayerSwitch) return;

        _crosshair.SetActive(state == GameState.PlayerTurn);
        _playerNameScreen.SetActive(state == GameState.PlayerTurn);
        _playerHealthScreen.SetActive(state == GameState.PlayerTurn);
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
            StartCoroutine(StartPlayerTimer());
        }
        else if (state == GameState.GameOver)
        {
            StopAllCoroutines();
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
