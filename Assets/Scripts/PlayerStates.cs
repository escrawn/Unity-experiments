using UnityEngine;

public class PlayerStates
{
    public bool IsGrounded { get; set; }
    public bool IsJumpingButtonPressed { get; set; }
    public float Speed { get; set; }
    public float HorizontalAxisInput { get; set; }

    public float MaximumSlopeAngle { get; set; }

    public ContactPoint2D? CurrentSurfacePoint { get; set; }

    public bool IsOnAllowedSlope { get; set; }

    public void SetInitialStates()
    {
        IsJumpingButtonPressed = Input.GetKeyDown(KeyCode.Space);
        HorizontalAxisInput = Input.GetAxis("Horizontal");
    }

    public static PlayerStates InitializeMovementData(float speed, float maximumSlopeAngle)
    {
        return new PlayerStates
        {
            Speed = speed,
            MaximumSlopeAngle = maximumSlopeAngle
        };
    }
}