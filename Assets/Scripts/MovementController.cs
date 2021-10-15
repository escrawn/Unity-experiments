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
        Vector3 direction = CalculateDirectionVector(playerStates);
        Debug.DrawRay(gameObject.transform.position, -direction, Color.red);
        Debug.Log("Direction: " + direction);

        gameObject.transform.Translate(new Vector3(playerStates.HorizontalAxisInput * playerStates.Speed, 0, 0));

        HandleMovementDirection(gameObject, playerStates.HorizontalAxisInput);
    }

    private Vector3 CalculateDirectionVector(PlayerStates playerStates)
    {
        return new Vector3(playerStates.CurrentSurfacePoint.normal.x, playerStates.CurrentSurfacePoint.normal.y, 0);
    }


    private void HandleMovementDirection(GameObject gameObject, float horizontalAxis)
    {
        gameObject.transform.localScale = new Vector3(Mathf.Sign(horizontalAxis), 1, 1);
    }


    private void HandleRotation(GameObject gameObject, PlayerStates playerStates)
    {
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        float walkingAngle = CalculateRotationZ(gameObject);
        if (Mathf.Abs(walkingAngle) >= playerStates.MaximumSlopeAngle || !playerStates.IsGrounded)
        {
            ResetRotation(gameObject);
            FreezeRotation(rigidbody2D);
        }
        else
        {
            UnlockRotation(rigidbody2D);
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
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}