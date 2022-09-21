using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public Player playerPrefab;
    public List<Player> playerList;
    public bool test = false;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public void CreatePlayers(int amount)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Destroy(playerList[i].gameObject);
        }

        playerList.Clear();

        for (int i = 0; i < amount; i++)
        {
            Player player = Instantiate(playerPrefab, new Vector3(Random.Range(9f, -9f), 0.5f, Random.Range(9f, -9f)),
                Quaternion.identity);

            playerList.Add(player);

            if (i == 0)
            {
                player.GetComponent<PlayerMovement>().enabled = true;
                var camera = player.GetComponentInChildren<PlayerCamera>();

                camera.GetComponent<Camera>().enabled = true;
                camera.enabled = true;
            }
        }
        
        // GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchPlayer();
        }
    }

    public void SwitchPlayer()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            var playerMovement = playerList[i].GetComponent<PlayerMovement>();
            var playerCamera = playerList[i].GetComponentInChildren<PlayerCamera>();
            var camera = playerCamera.GetComponent<Camera>();

            playerMovement.enabled = !playerMovement.enabled;
            camera.enabled = !camera.enabled;
            playerCamera.enabled = !playerCamera.enabled;
        }
    }

    public void DisableAllPlayerInputs()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].GetComponent<PlayerMovement>().enabled = false;

            var playerCamera = playerList[i].GetComponentInChildren<PlayerCamera>();
            var camera = playerCamera.GetComponent<Camera>();

            camera.enabled = false;
            playerCamera.enabled = false;
        }
    }
}
