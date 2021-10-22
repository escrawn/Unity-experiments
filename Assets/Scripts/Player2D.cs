using System;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private MovementController _movementController;
    [SerializeField] private int maximumSlopeAngle = 60;
    private PlayerStates _playerStates;
    private Animator _animator;
    private static readonly int SpeedAnimatorId = Animator.StringToHash("Speed");
    private static readonly int VelocityYAnimatorId = Animator.StringToHash("VelocityY");
    private static readonly int IsGroundedAnimatorId = Animator.StringToHash("IsGrounded");
    public float speed = 0.1f;

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
        _playerStates.SetInitialStates();
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
        _animator.SetFloat(SpeedAnimatorId, Mathf.Abs(_playerStates.HorizontalAxisInput));
        _animator.SetFloat(VelocityYAnimatorId, _rigidbody2D.velocity.y);
        _animator.SetBool(IsGroundedAnimatorId, _playerStates.IsGrounded);
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        foreach (ContactPoint2D contact in c.contacts)
        {
            _playerStates.CurrentSurfacePoint = contact;
            if (IsAllowedSlope(contact))
            {
                _playerStates.IsGrounded = true;
                _playerStates.IsOnAllowedSlope = true;
                break;
            }

            _playerStates.IsOnAllowedSlope = false;
            if (_rigidbody2D.velocity.y < -0.01)
            {
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
        _playerStates.CurrentSurfacePoint = null;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        foreach (ContactPoint2D contact in c.contacts)
        {
            _playerStates.CurrentSurfacePoint = contact;
            if (IsAllowedSlope(contact))
            {
                _playerStates.IsGrounded = true;
                _playerStates.IsOnAllowedSlope = true;
                break;
            }
        }
    }

    public Vector2 GetCurrentVelocity()
    {
        return _rigidbody2D.velocity;
    }

    public float GetWalkingAngle()
    {
        if (_playerStates.CurrentSurfacePoint != null)
        {
            return Vector2.Angle(((ContactPoint2D) _playerStates.CurrentSurfacePoint).normal, Vector2.up);
        }

        return 0;
    }

    public Boolean IsGrounded()
    {
        return _playerStates.IsGrounded;
    }

    public Vector2 GetDirectionVector()
    {
        Vector2 normal = new Vector2(0, 0);
        if (_playerStates.CurrentSurfacePoint != null)
        {
            normal = ((ContactPoint2D) _playerStates.CurrentSurfacePoint).normal;
        }

        return new Vector2(Mathf.Sign(_playerStates.HorizontalAxisInput) * normal.x, normal.y);
    }

    public Vector3 GetLocalVector()
    {
        return transform.localRotation.eulerAngles;
    }
}