using UnityEngine;
using TMPro;

public sealed class Player : MonoBehaviour, IDamageable
{
    public new string name = "Name";
    public int health = 100;

    [Header("UI")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _healthText;

    [Header("Player")]
    [SerializeField] private MeshRenderer _playerMesh;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private PlayerCamera _playerCameraScript;

    private void Start()
    {
        _mainCamera = Camera.main;
        _canvas = GetComponentInChildren<Canvas>();

        TextMeshProUGUI[] texts = _canvas.GetComponentsInChildren<TextMeshProUGUI>();
        _nameText = texts[0];
        _healthText = texts[1];

        _nameText.text = name;
        _healthText.text = health.ToString();
    }

    public void Update()
    {
        // Look at the main camera
        Quaternion rotation = Camera.main.transform.rotation;
        _canvas.transform.LookAt(_canvas.transform.position + rotation * Vector3.forward, rotation * Vector3.up);

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
            _healthText.text = health.ToString();

            // Todo: Display how much damage you took and then have text move up and fade away
        }
    }

    public void Toggle()
    {
        _canvas.enabled = !_canvas.enabled;

        _playerMovement.enabled = !_playerMovement.enabled;
        _playerCameraScript.enabled = !_playerCameraScript.enabled;
        _playerCamera.enabled = !_playerCamera.enabled;
        _playerMesh.enabled = !_playerMesh.enabled;
    }
}