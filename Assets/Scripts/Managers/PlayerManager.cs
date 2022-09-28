using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManager : MonoBehaviour
{
    [SerializeField] private UIManger _uiManger;
    [SerializeField] private Rocket _rocketPrefab;

    public Player playerPrefab;
    public List<Player> playerList;
    public int activePlayerIndex = 0;
    public Camera activeCamera;
    private bool gameJustStarted;
    private GameState _gameState;

    private void OnEnable()
    {
        GameManager.OnGameStateChange += Test;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= Test;
    }

    private void Update()
    {
        if (_gameState != GameState.PlayerTurn)
        {
            return;
        }

        // Left Click
        if (Input.GetButtonDown("Fire1"))
        {
            Transform tr = playerList[activePlayerIndex].GetPlayerCameraTransform();
            Instantiate(_rocketPrefab, tr.transform.position, tr.transform.rotation, null);
        }

        // Right Click
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("grenade todo");
        }
    }

    private void Test(GameState state)
    {
        _gameState = state;

        if (state == GameState.Start)
        {
            CreatePlayers(8);
            gameJustStarted = true;

            GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
        }
        else if (state == GameState.GameOver)
        {
            StopAllCoroutines();
        }
        else if (state == GameState.PlayerTurn)
        {
            // If the game has just started, start the timer only
            // When all players were created we set the first player to being able to move around and made all canvas look at active player
            // Without the variable, we would switch directly to the second player
            if (gameJustStarted)
            {
                gameJustStarted = false;
                StartCoroutine(StartTimer(50));
                return;
            }

            SwitchPlayer();
            UpdateCameraLookAt();
            StartCoroutine(StartTimer(50));
        }
    }

    public void CreatePlayers(int amount)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Destroy(playerList[i]);
        }

        playerList.Clear();

        for (int i = 0; i < amount; i++)
        {
            // Instantiate player and set its name
            Player player = Instantiate(playerPrefab, new Vector3(Random.Range(19f, -19f), 0.5f, Random.Range(19f, -19f)), Quaternion.identity);

            player.name = "Player " + (i + 1);
            player.index = i;
            player.playerManager = this;

            // Add the player to a list
            playerList.Add(player);

            // ACtivate movement and camera for the first player
            if (activePlayerIndex == i)
            {
                player.Toggle();
                activeCamera = player.GetComponentInChildren<Camera>();
            }
            else
            {
                player.SetCamera(activeCamera);
            }

            player.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
        }
    }

    public void SwitchPlayer()
    {
        if (playerList[activePlayerIndex] != null)
        {
            playerList[activePlayerIndex].Toggle(); 
        }

        do
        {
            activePlayerIndex++;
            activePlayerIndex %= playerList.Count;
        } 
        while (playerList[activePlayerIndex] == null);

        // Turns on the playermovement, camera, and etc
        playerList[activePlayerIndex].Toggle();
    }

    public void UpdateCameraLookAt()
    {
        // Update the activeCamera for the new player
        activeCamera = playerList[activePlayerIndex].GetComponentInChildren<Camera>();

        // Update it for all the players
        for (int i = 0; i < playerList.Count; i++)
        {
            if (activePlayerIndex != i && playerList[i] != null)
            {
                playerList[i].SetCamera(activeCamera);
            }
        }
    }

    public void RemovePlayer(int index)
    {
        playerList[index] = null;

        int playersAlive = 0;

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i] != null)
            {
                playersAlive++;
            }
        }

        if (playersAlive <= 1)
        {
            GameManager.Instance.UpdateGameState(GameState.GameOver);
        }
    }

    public string GetActivePlayerHealth()
    {
        return playerList[activePlayerIndex].health.ToString();
    }

    private IEnumerator StartTimer(int seconds)
    {
        WaitForSeconds delay = new WaitForSeconds(1f);
        _uiManger.SetPlayerTimer("", true);

        while (seconds > -1)
        {
            var timeSpan = System.TimeSpan.FromSeconds(seconds);

            _uiManger.SetPlayerTimer(string.Format("{0:D2}", timeSpan.Seconds));

            yield return delay;

            seconds--;
        }

        _uiManger.SetPlayerTimer("", false);

        // Once timer reached 0, we switch player
        StopAllCoroutines();
        GameManager.Instance.UpdateGameState(GameState.Test);
    }
}
