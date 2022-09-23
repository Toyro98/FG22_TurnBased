using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player playerPrefab;
    public List<Player> playerList;
    public int activePlayerIndex = 0;
    public Camera activeCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchPlayer();
        }
    }

    public void CreatePlayers(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // Instantiate player and set its name
            Player player = Instantiate(playerPrefab, new Vector3(Random.Range(19f, -19f), 0.5f, Random.Range(19f, -19f)), Quaternion.identity);
            player.name = "Player " + (i + 1);

            // Add the player to a list
            playerList.Add(player);

            // Active movement and camera for the first player and set the activeCamera 
            if (activePlayerIndex == i)
            {
                player.Toggle();
                activeCamera = player.GetComponentInChildren<Camera>();
            }
            else
            {
                player.SetCamera(activeCamera);
            }

            player.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
        
        // GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
    }

    public void SwitchPlayer()
    {
        // Turn off playermovement, camera, and etc for current active player index and then increase the index
        playerList[activePlayerIndex++].Toggle();

        activePlayerIndex %= playerList.Count;

        playerList[activePlayerIndex].Toggle();
        UpdateCameraLookAt();
    }

    private void UpdateCameraLookAt()
    {
        activeCamera = playerList[activePlayerIndex].GetComponentInChildren<Camera>();

        for (int i = 0; i < playerList.Count; i++)
        {
            if (activePlayerIndex != i)
            {
                playerList[i].SetCamera(activeCamera);
            }
        }
    }

    public void DestroyAllPlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Destroy(playerList[i].gameObject);
        }

        playerList.Clear();
    }
}
