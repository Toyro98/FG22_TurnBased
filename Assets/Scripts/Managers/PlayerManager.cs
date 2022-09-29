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
    private GameState _gameState;

    private void OnEnable()
    {
        GameManager.OnGameStateChange += GameStateChange;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= GameStateChange;
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
            Debug.Log("todo");
        }
    }

    private void GameStateChange(GameState state)
    {
        _gameState = state;

        if (state == GameState.PlayerSwitch)
        {
            SwitchPlayer();
            UpdateCameraLookAt();

            GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
        }
    }

    public void CreatePlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Destroy(playerList[i]);
        }

        playerList.Clear();

        for (int i = 0; i < GameManager.Instance.GameSettings.players; i++)
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
        else
        {
            GameManager.Instance.UpdateGameState(GameState.PlayerSwitch);
        }
    }

    public string GetActivePlayerHealth()
    {
        return playerList[activePlayerIndex].health.ToString();
    }
}
