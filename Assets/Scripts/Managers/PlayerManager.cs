using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManager : MonoBehaviour
{
    [SerializeField] private UIManger _uiManger;
    [SerializeField] private Projectile _rocketPrefab;
    [SerializeField] private Projectile _grenadePrefab;
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private PlayerShooting _playerShooting;
    [SerializeField] private List<Player> _playerList;
    [SerializeField] private List<Transform> _possibleSpawnLocations;
    private List<Transform> _spawnedLocations = new List<Transform>();

    public int activePlayerIndex = 0;
    public Camera activeCamera;

    private void OnEnable()
    {
        GameManager.OnGameStateChange += GameStateChange;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= GameStateChange;
    }

    private void GameStateChange(GameState state)
    {
        _playerShooting.enabled = state == GameState.PlayerTurn;

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
            Player player = Instantiate(_playerPrefab, FindUnusedSpawnLocation());
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
    
    public Transform GetActivePlayerCameraTransform()
    {
        return _playerList[activePlayerIndex].playerCamera.transform;
    }

    private Transform FindUnusedSpawnLocation()
    {
        Transform position;
        bool alreadySpawned;

        while (true)
        {
            position = _possibleSpawnLocations[Random.Range(0, _possibleSpawnLocations.Count)];
            alreadySpawned = false;

            for (int i = 0; i < _spawnedLocations.Count; i++)
            {
                if (position.position == _spawnedLocations[i].position)
                {
                    alreadySpawned = true;
                }
            }

            if (!alreadySpawned)
            {
                _spawnedLocations.Add(position);
                break;
            }
        }

        return position;
    }
}
