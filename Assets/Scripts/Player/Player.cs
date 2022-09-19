using UnityEngine;
using TMPro;

public sealed class Player : MonoBehaviour, IDamageable
{
    public new string name = "Nameless";
    public int health = 100;
    
    // Used to display player's name and health
    private Camera _mainCamera;
    private RectTransform _canvas;
    private TMP_Text _nameText;
    private TMP_Text _healthText;

    private void Start()
    {
        _mainCamera = Camera.main;
        _canvas = GetComponentInChildren<RectTransform>();

        TMP_Text[] texts = _canvas.GetComponentsInChildren<TMP_Text>();
        _nameText = texts[0];
        _healthText = texts[1];
        
        _nameText.text = name;
        _healthText.text = health.ToString();
    }

    public void Update()
    {
        // Look at the main camera
        Quaternion rotation = _mainCamera.transform.rotation;
        _canvas.LookAt(_canvas.transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        
        // Temp
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(Random.Range(1, 17));
        }
    }
    
    public void TakeDamage(int amount)
    {
        health -= amount;
        _healthText.text = health.ToString();
        
        // Todo: Display how much damage you took and then have text move up and fade away
    }
}
