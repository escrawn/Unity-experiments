using UnityEngine;

public class Player2D : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private MovementController _movementController;
    public long speed = 10L;
    [SerializeField] private int maximumSlopeAngle = 60;
    private MovementData _movementData;
    private Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int VelocityY = Animator.StringToHash("VelocityY");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");

    private void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _movementController = new MovementController();
        _animator = gameObject.GetComponent<Animator>();
        _movementData = MovementData.InitializeMovementData(speed, maximumSlopeAngle);
    }

    //Use Update for input interactions and instant physics like jumps.
    private void Update()
    {
        _movementData.SetDynamicsMovementDataAttributes();
        HandleJump();
    }

    //Use FixedUpdate for rigidBody and physics interactions.
    private void FixedUpdate()
    {
        _movementController.Move(gameObject, _movementData);
    }

    private void LateUpdate()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        _animator.SetFloat(Speed, Mathf.Abs(_movementData.HorizontalAxisInput));
        _animator.SetFloat(VelocityY, _rigidbody2D.velocity.y);
        _animator.SetBool(IsGrounded, _movementData.IsGrounded);
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        foreach (ContactPoint2D contact in c.contacts)
        {
            if (IsAllowedSlope(contact))
            {
                _movementData.IsGrounded = true;
               // _movementData.IsOnWall = false;
                break;
            }

            //_movementData.IsOnWall = true;
            _movementData.IsGrounded = false;
        }
    }

    private void HandleJump()
    {
        if (_movementData.IsJumpingButtonPressed && _movementData.IsGrounded)
        {
            _rigidbody2D.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
    }

    private bool IsAllowedSlope(ContactPoint2D contact)
    {
        Debug.Log("Normal: " + contact.normal);
        _movementData.CurrentSurfacePoint = contact;
        float slopeAngle = Vector2.Angle(contact.normal, Vector2.up);
        Debug.Log("Slope Angle: " + slopeAngle);
        return slopeAngle >= 0 && slopeAngle <= maximumSlopeAngle;
    }

    private void OnCollisionExit2D(Collision2D c)
    {
        _movementData.IsGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        _movementData.IsGrounded = true;
    }
}