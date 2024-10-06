using UnityEngine;

public class SmoothFollow2D : MonoBehaviour
{
    public Transform followTarget; 
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float delayTime = 1.0f;

    private Vector3 velocity = Vector3.zero;
    private bool shouldFollow = false;

    void Start()
    {
        if (followTarget == null)
        {
            Debug.LogError("No target set for SmoothFollow2D script.");
        }
    }

    void LateUpdate()
    {
        if (!shouldFollow)
        {
            delayTime -= Time.deltaTime;
            if (delayTime <= 0f)
            {
                shouldFollow = true;
            }
        }

        if (shouldFollow && followTarget != null)
        {
            // Get the target position with offset, but keeping the Z position of the camera (for 2D)
            Vector3 desiredPosition = new Vector3(followTarget.position.x + offset.x, followTarget.position.y + offset.y, transform.position.z);

            // Smoothly move the camera towards that position
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}