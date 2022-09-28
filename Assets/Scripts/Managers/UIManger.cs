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

    private void OnEnable()
    {
        GameManager.OnGameStateChange += Time;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= Time;
    }

    public void SetPlayerTimer(string text = "", bool enabled = true)
    {
        _playerTimer.text = text;
        _playerTimerScreen.SetActive(enabled);
    }

    private void Time(GameState state)
    {
        _crosshair.SetActive(state == GameState.PlayerTurn);
        _playerHealthScreen.SetActive(state == GameState.PlayerTurn);
        _gameoverScreen.SetActive(state == GameState.GameOver);

        if (state == GameState.Start)
        {
            _gameTimerScreen.SetActive(true);
            StartCoroutine(Timer(45));
        }
        else if (state == GameState.PlayerTurn)
        {
            _playerHealth.text = _playerManager.GetActivePlayerHealth();
        }
        else if (state == GameState.GameOver)
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator Timer(int minutes)
    {
        int time = 60 * minutes;
        WaitForSeconds delay = new WaitForSeconds(1f);
        
        while (time > -1)
        {
            var timeSpan = TimeSpan.FromSeconds(time);
            _gameTimer.text = string.Format("{0:D}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            
            yield return delay;
            
            time--;
        }

        // Once timer reached 0, we update the game state
        GameManager.Instance.UpdateGameState(GameState.GameOver);
    }
}
