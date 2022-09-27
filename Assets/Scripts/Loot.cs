using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
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
        if (state == GameState.Test)
        {
            Debug.Log("<< Loot Drop >>");
            GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
        }
    }
}
