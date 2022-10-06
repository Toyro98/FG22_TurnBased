using UnityEngine;

public sealed class PlayerCamera : MonoBehaviour
{
    public Transform playerBody;
    public float sensitivity = 2f;

    private float _yaw;
    private float _pitch;
    private float _angle = 0f;

    private void Update()
    {
        if (GameManager.IsGamePaused) return;

        // Mouse input
        _yaw = Input.GetAxis("Mouse X") * sensitivity;
        _pitch = Input.GetAxis("Mouse Y") * sensitivity;

        // Lock the angle
        _angle -= _pitch;
        _angle = Mathf.Clamp(_angle, -90f, 90f);

        // Rotates the camera and player body
        transform.localRotation = Quaternion.Euler(_angle, 0f, 0f);
        playerBody.transform.Rotate(Vector3.up * _yaw);
    }
}