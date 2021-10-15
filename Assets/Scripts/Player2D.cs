using UnityEngine;

public class Player2D : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private MovementController _movementController;
    [SerializeField] private int maximumSlopeAngle = 60;
    private PlayerStates _playerStates;
    private Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int VelocityY = Animator.StringToHash("VelocityY");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    public long speed = 10L;

    private void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _movementController = new MovementController();
        _animator = gameObject.GetComponent<Animator>();
        _playerStates = PlayerStates.InitializeMovementData(speed, maximumSlopeAngle);
    }

    //Use Update for input interactions and instant physics like jumps.
    private void Update()
    {
        _playerStates.SetDynamicsMovementDataAttributes();
        HandleJump();
    }

    //Use FixedUpdate for rigidBody and physics interactions.
    private void FixedUpdate()
    {
        _movementController.Move(gameObject, _playerStates);
    }

    private void LateUpdate()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        _animator.SetFloat(Speed, Mathf.Abs(_playerStates.HorizontalAxisInput));
        _animator.SetFloat(VelocityY, _rigidbody2D.velocity.y);
        _animator.SetBool(IsGrounded, _playerStates.IsGrounded);
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        foreach (ContactPoint2D contact in c.contacts)
        {
            if (IsAllowedSlope(contact))
            {
                _playerStates.IsGrounded = true;
                _playerStates.IsOnAllowedSlope = true;
                // _movementData.IsOnWall = false;
                break;
            }

            _playerStates.IsOnAllowedSlope = false;
            Debug.Log("Velocity Y: " + _rigidbody2D.velocity.y);
            if (_rigidbody2D.velocity.y == 0)
            {
                //_movementData.IsOnWall = true;
                _playerStates.IsGrounded = false;
                break;
            }

            _playerStates.IsGrounded = true;
        }
    }

    private void HandleJump()
    {
        if (_playerStates.IsJumpingButtonPressed && _playerStates.IsGrounded && _playerStates.IsOnAllowedSlope)
        {
            _rigidbody2D.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
    }

    private bool IsAllowedSlope(ContactPoint2D contact)
    {
        _playerStates.CurrentSurfacePoint = contact;
        float slopeAngle = CalculateSlopeAngle(contact);
        return slopeAngle >= 0 && slopeAngle <= maximumSlopeAngle;
    }
    
    private static float CalculateSlopeAngle(ContactPoint2D contact)
    {
        return Vector2.Angle(contact.normal, Vector2.up);
    }

    private void OnCollisionExit2D(Collision2D c)
    {
        _playerStates.IsGrounded = false;
    }
}