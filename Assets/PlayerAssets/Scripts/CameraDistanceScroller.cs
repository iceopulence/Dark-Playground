using UnityEngine;

public class CameraDistanceScroller : MonoBehaviour
{
    public CameraPropertyController cameraPropertyController;
    public float scrollSensitivity = 1f; // Sensitivity multiplier for scroll input
    public float minCameraDistance = 5f;  // Minimum camera distance
    public float maxCameraDistance = 15f; // Maximum camera distance
    public float cullingThreshold = 7f;  // Distance threshold for culling a layer
    public int layerToCull = 8;          // Layer number to cull
    public Camera mainCamera;            // Reference to the main camera

    private int originalCullingMask;     // To store the original culling mask

    void Start()
    {
        if (mainCamera != null)
        {
            // Store the original culling mask at start
            originalCullingMask = mainCamera.cullingMask;
        }
    }

    void Update()
    {
        if (cameraPropertyController != null)
        {
            HandleCameraDistanceInput();
            UpdateCullingMask();
        }
    }

    private void HandleCameraDistanceInput()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        float newTargetDistance = Mathf.Clamp(cameraPropertyController.GetTargetCameraDistance() + scrollInput, minCameraDistance, maxCameraDistance);
        cameraPropertyController.SetTargetCameraDistance(newTargetDistance);
    }

    private void UpdateCullingMask()
    {
        if (mainCamera != null)
        {
            if (cameraPropertyController.GetTargetCameraDistance() < cullingThreshold)
            {
                // Cull the specified layer
                mainCamera.cullingMask = originalCullingMask & ~(1 << layerToCull);
            }
            else
            {
                // Restore the original culling mask
                mainCamera.cullingMask = originalCullingMask;
            }
        }
    }
}
