using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManager : MonoBehaviour
{
    [SerializeField] private UIManger _uiManger;
    [SerializeField] private Rocket _rocketPrefab;
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private List<Player> _playerList;

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
            Transform camera = _playerList[activePlayerIndex].playerCamera.transform;
            Instantiate(_rocketPrefab, camera.transform.position + camera.transform.forward * 2, camera.transform.rotation, null);
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
        int playerCount = GameManager.Instance.GameSettings.playerCount;
        int health = GameManager.Instance.GameSettings.playerHealth;

        for (int i = 0; i < playerCount; i++)
        {
            // TODO: Create spawnpoints on the map and pick a random location from it. If a player has already spawned there, look for a new location
            Player player = Instantiate(_playerPrefab, new Vector3(Random.Range(19f, -19f), 0.5f, Random.Range(19f, -19f)), Quaternion.identity);
            player.name = "Player " + (i + 1);
            player.index = i;
            player.health = health;
            player.playerManager = this;

            _playerList.Add(player);

            // Activate movement and camera for the first player only
            // Otherwise focus the camera to the first player for other players
            if (activePlayerIndex == i)
            {
                player.Toggle();
                activeCamera = player.playerCamera;
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
        // Turn off the movement and camera for the active player
        if (_playerList[activePlayerIndex] != null)
        {
            _playerList[activePlayerIndex].Toggle(); 
        }

        // Increase the index by 1 and check if the player exist
        do
        {
            activePlayerIndex++;
            activePlayerIndex %= _playerList.Count;
        } 
        while (_playerList[activePlayerIndex] == null);

        // The current player index is alive and we turn on movement and camera for the player
        _playerList[activePlayerIndex].Toggle();
    }

    public void UpdateCameraLookAt()
    {
        // Update the activeCamera for the new player
        activeCamera = _playerList[activePlayerIndex].playerCamera;

        // Update it for all the players so it looks at the active player
        for (int i = 0; i < _playerList.Count; i++)
        {
            // Don't update if it's the same person and player doesn't exist
            if (activePlayerIndex != i && _playerList[i] != null)
            {
                _playerList[i].SetCamera(activeCamera);
            }
        }
    }

    public void RemovePlayer(int index)
    {
        _playerList[index] = null;
    }

    public string GetActivePlayerName()
    {
        return _playerList[activePlayerIndex].name;
    }

    public string GetActivePlayerHealth()
    {
        return _playerList[activePlayerIndex].health.ToString();
    }
}
