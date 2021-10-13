using UnityEngine;

public class Camera2D : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothTime = 0.1F;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [SerializeField] private float offsetZ = -3;
    public Vector3 currentVelocity;
    private void FixedUpdate()
    {
        FollowTarget();
    }


    void FollowTarget()
    {
        Vector3 targetPosition = target.transform.TransformPoint(new Vector3(offsetX, offsetY, offsetZ));
        currentVelocity = Vector3.zero;
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}