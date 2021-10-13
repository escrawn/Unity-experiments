using UnityEngine;

public class MovementData
{
    public bool IsGrounded { get; set; }
    public bool IsJumpingButtonPressed { get; set; }
    public long Speed { get; set; }
    public float HorizontalAxisInput { get; set; }

    public float MaximumSlopeAngle { get; set; }

    public ContactPoint2D CurrentSurfacePoint { get; set; }

    public void SetDynamicsMovementDataAttributes()
    {
        IsJumpingButtonPressed = Input.GetKeyDown(KeyCode.Space);
        HorizontalAxisInput = Input.GetAxis("Horizontal");
    }

    public static MovementData InitializeMovementData(long speed, float maximumSlopeAngle)
    {
        return new MovementData
        {
            Speed = speed,
            MaximumSlopeAngle = maximumSlopeAngle
        };
    }
}