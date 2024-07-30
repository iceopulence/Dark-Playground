using UnityEngine;

public class MainMenuCameraController : MonoBehaviour
{
    public Vector3 cameraOffset = new Vector3(0, 5, -10); // Offset of the camera from the target
    public float smoothTime = 0.3F; // Time for the camera to smooth into position
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void LateUpdate()
    {
        MoveAndRotate(targetPosition, targetRotation);
    }

    private void MoveAndRotate(Vector3 position, Quaternion rotation)
    {
        transform.position = Vector3.Lerp(transform.position, position, smoothTime * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, smoothTime * Time.deltaTime);
    }

    public void SetTargetTransform(Transform newTarget)
    {
        targetPosition = newTarget.position + cameraOffset;
        targetRotation = newTarget.rotation;
    }
    
    public void SetTargetTransform(Transform newTarget, float smoothTime)
    {
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
        targetRotation = Quaternion.LookRotation(forwardDirection);
    }

    public void SetSmoothTime(float smoothTime)
    {
        this.smoothTime = smoothTime;
    }
}
