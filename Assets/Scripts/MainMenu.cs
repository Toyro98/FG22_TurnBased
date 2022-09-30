using UnityEngine;
using TMPro;

public sealed class MainMenu : MonoBehaviour
{
    public GameSettings gameSettings;
    public TMP_Dropdown playerCountDropdown;
    public TMP_Dropdown gameTimeDropdown;
    public TMP_Dropdown playerTimeDropdown;
    public TMP_Dropdown playerHealthDropdown;

    public void Init()
    {
        gameSettings.playerCount = int.Parse(playerCountDropdown.options[playerCountDropdown.value].text);
        gameSettings.gameTime = int.Parse(gameTimeDropdown.options[gameTimeDropdown.value].text);
        gameSettings.timePerPlayer = int.Parse(playerTimeDropdown.options[playerTimeDropdown.value].text);
        gameSettings.playerHealth = int.Parse(playerHealthDropdown.options[playerHealthDropdown.value].text);
    }
}
