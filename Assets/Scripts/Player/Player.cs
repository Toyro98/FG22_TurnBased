using UnityEngine;
using TMPro;

public sealed class Player : MonoBehaviour, IDamageable
{
    public new string name = "Nameless";
    public int health = 100;

    // Used to display player's name and health
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private RectTransform _canvas;
    public TMP_Text nameText;
    public TMP_Text healthText;

    private void Start()
    {
        _mainCamera = Camera.main;
        _canvas = GetComponentInChildren<RectTransform>();

        TMP_Text[] texts = _canvas.GetComponentsInChildren<TMP_Text>();
        nameText = texts[0];
        healthText = texts[1];
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

        if (health <= 0)
        {
            GameManager.Instance.UpdateGameState(GameState.GameOver);
        }
        else
        {
            healthText.text = health.ToString();

            // Todo: Display how much damage you took and then have text move up and fade away
        }
    }
}