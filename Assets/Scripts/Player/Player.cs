using UnityEngine;
using TMPro;

public sealed class Player : MonoBehaviour, IDamageable
{
    public new string name = "Name";
    public int health = 100;

    [Header("UI")]
    [SerializeField] private Camera _cameraLookAt;
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
        _nameText.text = name;
        _healthText.text = health.ToString();
    }

    public void Update()
    {
        Quaternion rotation = _cameraLookAt.transform.rotation;
        _canvas.transform.LookAt(_canvas.transform.position + rotation * Vector3.forward, rotation * Vector3.up);
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
        // Hides the canvas above the active player but show the canvas above other players
        _canvas.enabled = !_canvas.enabled;

        _playerMovement.canMoveAround = !_playerMovement.canMoveAround;

        _playerCameraScript.enabled = !_playerCameraScript.enabled;
        _playerCamera.enabled = !_playerCamera.enabled;
        _playerMesh.enabled = !_playerMesh.enabled;
    }

    public void SetCamera(Camera newCamera)
    {
        _cameraLookAt = newCamera;
    }
}