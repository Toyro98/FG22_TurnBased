using UnityEngine;

public sealed class PlayerCamera : MonoBehaviour
{
    public float sensitivity = 2f;
    private float _yaw;
    private float _pitch;
    private float _angle = 0f;
    public Transform playerBody;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Gets the mouse input
        _yaw = Input.GetAxis("Mouse X") * sensitivity; // left right
        _pitch = Input.GetAxis("Mouse Y") * sensitivity; // up down

        // Locks the angle
        _angle -= _pitch;
        _angle = Mathf.Clamp(_angle, -90f, 90f);

        // Rotates the camera and player body
        transform.localRotation = Quaternion.Euler(_angle, 0f, 0f);
        playerBody.transform.Rotate(Vector3.up * _yaw);
    }
}