using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerMovement : MonoBehaviour
{
    private const float GRAVITY = -9.81f * 2f;
    
    public float speed = 3f;
    public float jumpHeight = 1f;
    public bool canMoveAround = false;
    
    private CharacterController _controller;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector3 groundDistance;

    private Vector3 _velocity;
    private Vector3 _movement;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (IsGrounded() && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        if (canMoveAround)
        {
            _movement = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
            _velocity.y += GRAVITY * Time.deltaTime;

            _controller.Move(speed * Time.deltaTime * _movement);
            _controller.Move(_velocity * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
            }
        }
        else
        {
            _velocity.y += GRAVITY * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckBox(groundCheck.position, groundDistance, Quaternion.identity, groundLayer);
    }
}