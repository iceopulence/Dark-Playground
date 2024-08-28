using UnityEngine;

public class AnimatedCamera : MonoBehaviour
{
    public Vector3 cameraOffset = new Vector3(0, 5, -10); // Offset of the camera from the target
    public float smoothTime = 0.3F; // Time for the camera to smooth into position
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public float positionThreshold = 0.5f;
    public float rotationThreshold = 15f;

    public bool isAnimating = false;

    void Start()
    {
        SetTargetTransform(transform); // start camera in its starting place    
    }

    private void LateUpdate()
    {
        MoveAndRotate(targetPosition, targetRotation);

        isAnimating = Vector3.Distance(transform.position, targetPosition) > positionThreshold 
                        || Vector3.Angle(transform.eulerAngles, targetRotation.eulerAngles) > rotationThreshold;  
    }

    private void MoveAndRotate(Vector3 position, Quaternion rotation)
    {
        isAnimating = true;
        transform.position = Vector3.Lerp(transform.position, position, smoothTime * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, smoothTime * Time.deltaTime);
    }

    public void SetTargetTransform(Transform newTarget)
    {
        isAnimating = true;
        targetPosition = newTarget.position + cameraOffset;
        targetRotation = newTarget.rotation;
    }
    
    public void SetTargetTransform(Transform newTarget, float smoothTime)
    {
        isAnimating = true;
        targetPosition = newTarget.position + cameraOffset;
        targetRotation = newTarget.rotation;
        this.smoothTime = smoothTime;
    }

    public void SetTargetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition + cameraOffset;
    }

    public void SetTargetRotation(Vector3 forwardDirection)
    {
        targetRotation = Quaternion.Euler(forwardDirection);
    }

    public void SetTargetRotation(Quaternion newRotation)
    {
        targetRotation = newRotation;
    }

    public void SetSmoothTime(float smoothTime)
    {
        this.smoothTime = smoothTime;
    }
}

[System.Serializable]
public class CameraTarget
{
    public Transform transform;
    public float smoothTime;
}