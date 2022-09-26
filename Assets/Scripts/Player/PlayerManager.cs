using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private UIManger uiManger;
    public Player playerPrefab;
    public List<Player> playerList;
    public int activePlayerIndex = 0;
    public Camera activeCamera;

    private void Start()
    {
        uiManger = GetComponent<UIManger>();
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
        
        // GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
    }

    public void SwitchPlayer()
    {
        StopAllCoroutines();

        // Turn off playermovement, camera, and etc for current active player index and then increase the index
        playerList[activePlayerIndex++].Toggle();

        activePlayerIndex %= playerList.Count;

        playerList[activePlayerIndex].Toggle();
    }

    public void UpdateCameraLookAt()
    {
        activeCamera = playerList[activePlayerIndex].GetComponentInChildren<Camera>();

        for (int i = 0; i < playerList.Count; i++)
        {
            if (activePlayerIndex != i)
            {
                playerList[i].SetCamera(activeCamera);
            }
        }

        StartCoroutine(Timer(45));
    }

    public void DestroyAllPlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Destroy(playerList[i].gameObject);
        }

        playerList.Clear();
    }

    public IEnumerator Timer(int time)
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
        SwitchPlayer();
        UpdateCameraLookAt();
    }
}
