using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameSettings
{
    public int totalPlayTime = 45;
    public int timePerPlayer = 15;
    public int players = 4;

    public GameSettings() {}

    public GameSettings(int totalPlayTime, int timePerPlayer, int players)
    {
        this.totalPlayTime = totalPlayTime;
        this.timePerPlayer = timePerPlayer;
        this.players = players;
    }
}
