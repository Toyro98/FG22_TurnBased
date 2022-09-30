using UnityEngine;

public sealed class GameSettings : MonoBehaviour
{
    public int gameTime = 45;
    public int timePerPlayer = 15;
    public int playerCount = 4;
    public int playerHealth = 100;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
