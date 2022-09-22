using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player playerPrefab;
    public List<Player> playerList;
    public int activePlayerIndex = 0;

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
            Player player = Instantiate(playerPrefab, new Vector3(Random.Range(19f, -19f), 0.5f, Random.Range(19f, -19f)),
                Quaternion.identity);

            player.name = "Player " + (i + 1);

            playerList.Add(player);

            if (i == activePlayerIndex)
            {
                player.Toggle();
            }

            // player.mesh.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
        
        // GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
    }

    public void SwitchPlayer()
    {
        // Turn off playermovement, camera, and etc for current active player index and then increase the index
        playerList[activePlayerIndex++].Toggle();

        activePlayerIndex %= playerList.Count;

        playerList[activePlayerIndex].Toggle();
    }

    public void DisableAllPlayerInputs()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Destroy(playerList[i].gameObject);
            /*
            playerList[i].GetComponent<PlayerMovement>().enabled = false;

            var playerCamera = playerList[i].GetComponentInChildren<PlayerCamera>();
            var camera = playerCamera.GetComponent<Camera>();

            camera.enabled = false;
            playerCamera.enabled = false;*/
        }
    }
}
