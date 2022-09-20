using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public Player playerPrefab;
    public List<Player> playerList;
    
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
        for (int i = 0; i < amount; i++)
        {
            Player player = Instantiate(playerPrefab, new Vector3(Random.Range(9f, -9f), 0.5f, Random.Range(9f, -9f)),
                Quaternion.identity);

            playerList.Add(player);
        }

        // GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
    }
}
