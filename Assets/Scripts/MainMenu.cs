using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameSettings gameSettings;
    public TMP_Dropdown playerCountDropdown;
    public TMP_Dropdown gameTimeDropdown;
    public TMP_Dropdown playerTimeDropdown;
    public TMP_Dropdown playerHealthDropdown;

    public void Init()
    {
        gameSettings.players = int.Parse(playerCountDropdown.options[playerCountDropdown.value].text);
        gameSettings.totalPlayTime = int.Parse(gameTimeDropdown.options[gameTimeDropdown.value].text);
        gameSettings.timePerPlayer = int.Parse(playerTimeDropdown.options[playerTimeDropdown.value].text);
        gameSettings.playerHealth = int.Parse(playerHealthDropdown.options[playerHealthDropdown.value].text);
    }
}
