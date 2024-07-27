using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPropertyController : MonoBehaviour
{
    [SerializeField]
    private float targetFOV = 60f;
    [SerializeField]
    private float fovLerpSpeed = 5f; // Speed at which FOV lerps to its target

    [SerializeField]
    private float cameraDistanceLerpSpeed = 5f; // Speed at which camera distance lerps to its target

    [SerializeField]
    private float shoulderOffsetLerpSpeed = 5f; // Speed at which shoulder offset lerps to its target

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private Cinemachine3rdPersonFollow thirdPersonFollow;

    [SerializeField] private Vector3 targetShoulderOffset = Vector3.zero;
    [SerializeField] private float targetCameraDistance = 10f;

    private float currentFOV;
    private float currentCameraDistance;
    private Vector3 currentShoulderOffset;

    void Awake()
    {
        if(cinemachineVirtualCamera != null)
        {
            thirdPersonFollow = cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            if (thirdPersonFollow != null)
            {
                // Initialize current values with the initial settings
                currentFOV = cinemachineVirtualCamera.m_Lens.FieldOfView;
                currentCameraDistance = thirdPersonFollow.CameraDistance;
                currentShoulderOffset = thirdPersonFollow.ShoulderOffset;
            }
        }
    }

    void Update()
    {
        
        
        PropertyLerping();
    }

    public void PropertyLerping()
    {
        // Check if FOV has changed and lerp only if necessary
        if (cinemachineVirtualCamera != null && !Mathf.Approximately(currentFOV, targetFOV))
        {
            cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFOV, fovLerpSpeed * Time.deltaTime);
            currentFOV = cinemachineVirtualCamera.m_Lens.FieldOfView;
        }

        // Check if camera distance has changed and lerp only if necessary
        if (thirdPersonFollow != null && !Mathf.Approximately(currentCameraDistance, targetCameraDistance))
        {
            thirdPersonFollow.CameraDistance = Mathf.Lerp(thirdPersonFollow.CameraDistance, targetCameraDistance, cameraDistanceLerpSpeed * Time.deltaTime);
            currentCameraDistance = thirdPersonFollow.CameraDistance;
        }

        // Check if shoulder offset has changed and lerp only if necessary
        if (thirdPersonFollow != null && currentShoulderOffset != targetShoulderOffset)
        {
            thirdPersonFollow.ShoulderOffset = Vector3.Lerp(thirdPersonFollow.ShoulderOffset, targetShoulderOffset, shoulderOffsetLerpSpeed * Time.deltaTime);
            currentShoulderOffset = thirdPersonFollow.ShoulderOffset;
        }
    }

    public void SetTargetFOV(float newFOV)
    {
        targetFOV = newFOV;
        print("new target fov = " + newFOV);
    }

    public void SetTargetCameraDistance(float newDistance)
    {
        targetCameraDistance = newDistance;
        print("new target distance = " + newDistance);
    }

    public void SetTargetShoulderOffset(Vector3 newOffset)
    {
        targetShoulderOffset = newOffset;
        print("new target offset = " + newOffset);
    }

    public float GetTargetCameraDistance()
    {
        return targetCameraDistance;
    }
}
