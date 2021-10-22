using UnityEngine;

public class MovementController
{
    public void Move(GameObject gameObject, PlayerStates playerStates)
    {
        HandleHorizontalMovement(gameObject, playerStates);
        HandleRotation(gameObject, playerStates);
    }

    private void HandleHorizontalMovement(GameObject gameObject, PlayerStates playerStates)
    {
        //Debug.DrawRay(gameObject.transform.position, -playerStates.CurrentSurfacePoint.normal, Color.red);

        gameObject.transform.Translate(new Vector3(playerStates.HorizontalAxisInput * playerStates.Speed, 0, 0),
            Space.Self);

        HandleMovementDirection(gameObject, playerStates.HorizontalAxisInput);
    }

    private void HandleMovementDirection(GameObject gameObject, float horizontalAxis)
    {
        if (horizontalAxis != 0)
        {
            gameObject.transform.localScale = new Vector3(Mathf.Sign(horizontalAxis), 1, 1);
        }
    }


    private void HandleRotation(GameObject gameObject, PlayerStates playerStates)
    {
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        float walkingAngle = CalculateWalkingAngle(playerStates);

        if (!playerStates.IsGrounded)
        {
            ResetRotation(gameObject);
            //FreezeRotation(rigidbody2D);
        }

        if (walkingAngle < playerStates.MaximumSlopeAngle && playerStates.IsGrounded)
        {
            AdjustRotationToFloor(gameObject, playerStates);
            FreezeRotation(rigidbody2D);
        }
    }

    private float CalculateRotationZ(GameObject gameObject)
    {
        float angle = gameObject.transform.eulerAngles.z;
        return angle >= 160 ? angle - 360 : angle;
    }

    private void UnlockRotation(Rigidbody2D rigidbody2D)
    {
        rigidbody2D.constraints = RigidbodyConstraints2D.None;
    }

    private void FreezeRotation(Rigidbody2D rigidbody2D)
    {
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void ResetRotation(GameObject gameObject)
    {
        gameObject.transform.rotation =
            Quaternion.LookRotation(Vector3.forward, new Vector3(0, 0, 0));
    }

    private float CalculateWalkingAngle(PlayerStates playerStates)
    {
        Vector2 normal = new Vector2(0, 0);
        if (playerStates.CurrentSurfacePoint != null)
        {
            normal = ((ContactPoint2D) playerStates.CurrentSurfacePoint).normal;
        }

        return Vector2.Angle(normal, Vector2.up);
    }

    private void AdjustRotationToFloor(GameObject gameObject, PlayerStates playerStates)
    {
        Vector2 normal = new Vector2(0, 0);
        if (playerStates.CurrentSurfacePoint != null)
        {
            normal = ((ContactPoint2D) playerStates.CurrentSurfacePoint).normal;
        }

        gameObject.transform.rotation =
            Quaternion.LookRotation(Vector3.forward, normal);
    }
}