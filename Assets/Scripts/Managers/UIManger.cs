using System;
using System.Collections;
using UnityEngine;
using TMPro;

public sealed class UIManger : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private GameObject _gameoverScreen;
    [SerializeField] private GameObject _gameTimerScreen;
    [SerializeField] private TMP_Text _gameTimer;
    [SerializeField] private GameObject _playerTimerScreen;
    [SerializeField] private TMP_Text _playerTimer;
    [SerializeField] private GameObject _playerHealthScreen;
    [SerializeField] private TMP_Text _playerHealth;

    [Header("Settings")]
    [SerializeField] private GameObject _settingsMenu;

    private void OnEnable()
    {
        GameManager.OnGameStateChange += GameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= GameStateChanged;
    }

    public void SetPlayerTimer(string text = "", bool enabled = true)
    {
        _playerTimer.text = text;
        _playerTimerScreen.SetActive(enabled);
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.PlayerSwitch) return;

        //Debug.Log("state = " + state);
        //Debug.Log("1 _crosshair.activeInHierarchy = " + _crosshair.activeInHierarchy);

        _crosshair.SetActive(state == GameState.PlayerTurn);
        _playerHealthScreen.SetActive(state == GameState.PlayerTurn);
        _gameoverScreen.SetActive(state == GameState.GameOver);

        //Debug.Log("2 _crosshair.activeInHierarchy = " + _crosshair.activeInHierarchy);
        
        if (state == GameState.Start)
        {
            _gameTimerScreen.SetActive(true);
            StartCoroutine(StartGameTimer());
        }
        else if (state == GameState.PlayerTurn)
        {
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
        int time = GameManager.Instance.GameSettings.totalPlayTime * 60;
        WaitForSeconds delay = new WaitForSeconds(1f);

        // Game has started
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

        GameManager.Instance.UpdateGameState(GameState.GameOver);
    }

    private IEnumerator StartPlayerTimer()
    {
        int time = GameManager.Instance.GameSettings.timePerPlayer;
        WaitForSeconds delay = new WaitForSeconds(1f);
        _playerTimerScreen.SetActive(true);

        while (time > 0)
        {
            var timeSpan = TimeSpan.FromSeconds(time--);
            _playerTimer.text = string.Format("{0:D2}", timeSpan.Seconds);

            yield return delay;
        }

        _playerTimerScreen.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.PlayerSwitch);
    }
}
