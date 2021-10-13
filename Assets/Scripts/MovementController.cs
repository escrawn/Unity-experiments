using UnityEngine;

public class MovementController
{
    public void Move(GameObject gameObject, MovementData movementData)
    {
        HandleHorizontalMovement(gameObject, movementData);
        HandleRotation(gameObject, movementData);
    }

    private void HandleHorizontalMovement(GameObject gameObject, MovementData movementData)
    {
        Vector3 direction = CalculateDirectionVector(movementData);
        Debug.DrawRay(gameObject.transform.position, -direction, Color.red);

        gameObject.transform.position +=
            new Vector3(movementData.HorizontalAxisInput, 0, 0) * movementData.Speed *
            Time.deltaTime;

        // Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        // rigidbody2D.velocity = new Vector2(movementData.HorizontalAxisInput*6, -Physics.gravity.magnitude);

        // gameObject.transform.position +=
        //     new Vector3(movementData.HorizontalAxisInput, -direction.y, 0)
        //     * movementData.Speed
        //     * Time.deltaTime;


        HandleMovementDirection(gameObject, movementData.HorizontalAxisInput);
    }

    private Vector3 CalculateDirectionVector(MovementData movementData)
    {
        return movementData.IsGrounded
               && IsAllowedSlope(movementData.CurrentSurfacePoint, movementData)
               && !movementData.CurrentSurfacePoint.normal.y.Equals(1)
            ? new Vector3(0, movementData.CurrentSurfacePoint.normal.y, 0)
            : Vector3.zero;
    }

    private bool IsAllowedSlope(ContactPoint2D contact, MovementData movementData)
    {
        movementData.CurrentSurfacePoint = contact;
        float slopeAngle = Vector2.Angle(contact.normal, Vector2.up);
        return slopeAngle >= 0 && slopeAngle <= 60;
    }


    private void HandleMovementDirection(GameObject gameObject, float horizontalAxis)
    {
        if (horizontalAxis > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalAxis < 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleRotation(GameObject gameObject, MovementData movementData)
    {
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        float walkingAngle = CalculateRotationZ(gameObject);
        if (Mathf.Abs(walkingAngle) > movementData.MaximumSlopeAngle || !movementData.IsGrounded)
        {
            FreezeRotation(rigidbody2D);
            ResetRotation(gameObject);
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
        Quaternion rotation = gameObject.transform.rotation;
        rotation = Quaternion.Euler(new Vector3(rotation.x, rotation.y, 0));
        gameObject.transform.rotation = rotation;
    }
}