using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManager : MonoBehaviour
{
    private UIManger uiManger;
    public Player playerPrefab;
    public List<Player> playerList;
    public int activePlayerIndex = 0;
    public Camera activeCamera;
    private bool gameJustStarted;

    private void Start()
    {
        uiManger = GetComponent<UIManger>();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += Test;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= Test;
    }

    private void Test(GameState state)
    {
        if (state == GameState.Start)
        {
            CreatePlayers(4);
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
                StartCoroutine(StartTimer(5));
                return;
            }

            SwitchPlayer();
            UpdateCameraLookAt();
            StartCoroutine(StartTimer(5));
        }
    }

    public void CreatePlayers(int amount)
    {
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

    public void DestroyAllPlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Destroy(playerList[i]);
        }

        playerList.Clear();
    }

    private IEnumerator StartTimer(int time)
    {
        WaitForSeconds delay = new WaitForSeconds(1f);
        uiManger.playerTimerScreen.SetActive(true);

        while (time > -1)
        {
            var timeSpan = System.TimeSpan.FromSeconds(time);

            uiManger.playerTimer.text = string.Format("{0:D2}", timeSpan.Seconds);

            yield return delay;

            time--;
        }

        uiManger.playerTimerScreen.SetActive(false);

        // Once timer reached 0, we switch player
        StopAllCoroutines();
        GameManager.Instance.UpdateGameState(GameState.Test);
    }
}
