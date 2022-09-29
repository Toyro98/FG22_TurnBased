using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameSettings : MonoBehaviour
{
    public int totalPlayTime = 45;
    public int timePerPlayer = 15;
    public int players = 4;
    public int playerHealth = 100;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
