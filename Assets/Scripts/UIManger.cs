using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class UIManger : MonoBehaviour
{
    public static UIManger Instance { get; private set; }

    [SerializeField] private GameObject _gameoverScreen;
    [SerializeField] private TMP_Text _timer;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += Time;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= Time;
    }
    
    public void StartNewGame()
    {
        GameManager.Instance.UpdateGameState(GameState.Start);
    }

    private void Time(GameState state)
    {
        if (state == GameState.Start)
        {
            _gameoverScreen.SetActive(false);
            StartCoroutine(Timer());
        }
        else if (state == GameState.GameOver)
        {
            _gameoverScreen.SetActive(true);
            StopAllCoroutines();
        }
    }

    IEnumerator Timer()
    {
        // Temp
        int time = 60 * 25;
        WaitForSeconds delay = new WaitForSeconds(1f);
        
        while (time > -1)
        {
            var timeSpan = TimeSpan.FromSeconds(time);
            _timer.text = string.Format("{0:D}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            
            yield return delay;
            
            time--;
        }

        // Once timer reached 0, we update the game state
        GameManager.Instance.UpdateGameState(GameState.GameOver);
    }
}
