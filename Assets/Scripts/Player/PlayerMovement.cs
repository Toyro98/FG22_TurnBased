using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerMovement : MonoBehaviour
{
    private const float GRAVITY = -9.81f;
    
    public float speed = 4f;
    public float jumpHeight = 2.5f;
    public bool canMoveAround = false;
    
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDistance = 0.3f;

    [SerializeField] private Vector3 _velocity;
    [SerializeField] private Vector3 _movement;

    private void Update()
    {
        if (GameManager.IsGamePaused) 
            return;

        Movement();
    }

    private void Movement()
    {
        bool isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundLayer);

        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        if (canMoveAround)
        {
            // Input
            _movement = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
            _controller.Move(speed * Time.deltaTime * _movement);

            // Gravity
            _velocity.y += GRAVITY * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);

            // Jumping
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
            }
        }
        else
        {
            // Apply gravity even if we can't move
            _velocity.y += GRAVITY * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}