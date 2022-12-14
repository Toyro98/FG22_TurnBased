using UnityEngine;
using TMPro;

public sealed class Player : MonoBehaviour, IDamageable
{
    public PlayerManager playerManager;
    public Camera playerCamera;
    public new string name = "";
    public int health = 100;
    public int index = 0;

    [Header("UI")]
    [SerializeField] private Camera _cameraLookAt;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _healthText;

    [Header("Player")]
    [SerializeField] private MeshRenderer _playerMesh;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerCamera _playerCameraScript;

    private void Start()
    {
        _nameText.text = name;
        _healthText.text = health.ToString();
    }

    private void Update()
    {
        if (_cameraLookAt != null)
        {
            Quaternion rotation = _cameraLookAt.transform.rotation;
            _canvas.transform.LookAt(_canvas.transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        }
        else
        {
            _cameraLookAt = Camera.main;
        }
    }

    public void ToggleEnabledState()
    {
        // Hides the canvas above the active player when it's their turn. Otherwise, show it
        _canvas.enabled = !_canvas.enabled;

        // Instead of toggling on and off the player movement script,
        // we change a variable so the player can fall down if they jumped before the switch
        _playerMovement.canMoveAround = !_playerMovement.canMoveAround;

        // Toggle on or off scripts and player's mesh
        playerCamera.enabled = !playerCamera.enabled;
        _playerCameraScript.enabled = !_playerCameraScript.enabled;
        _playerMesh.enabled = !_playerMesh.enabled;
    }

    public void SetCameraLookAt(Camera newCamera)
    {
        _cameraLookAt = newCamera;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        _healthText.text = health.ToString();

        if (health <= 0)
        {
            playerManager.RemovePlayer(index);
            Destroy(gameObject);
        }
        else
        {
            playerManager.DisplayHitDamage(amount, transform);
        }
    }
}