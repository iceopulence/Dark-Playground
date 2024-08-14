// PlayerInteraction.cs
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3.0f;
    public LayerMask interactableLayer;
    public KeyCode interactKey;
    [SerializeField] private Image interactionIndicator;

    private Transform _lastHitTransform;
    [SerializeField] private Camera mainCam;

    void Start()
    {
        if (interactionIndicator != null)
        {
            interactionIndicator.enabled = false;
        }
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("Main camera is not set or found.");
        }
    }

    void Update()
    {
        if (mainCam != null)
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
            {
                print("maybe ill be tracer");
                if (_lastHitTransform == null || _lastHitTransform != hit.transform)
                {
                    _lastHitTransform = hit.transform;
                    if (interactionIndicator != null)
                    {
                        interactionIndicator.enabled = true;
                    }
                }

                if (Input.GetKeyDown(interactKey))
                {
                    IInteractable interactableObject = hit.collider.GetComponent<IInteractable>();
                    print("Im already tracer");
                    if (interactableObject != null)
                    {
                        print("what about widdowmaker");
                        interactableObject.OnInteract();
                    }
                }
            }
            else
            {
                if (_lastHitTransform != null)
                {
                    _lastHitTransform = null;
                    if (interactionIndicator != null)
                    {
                        interactionIndicator.enabled = false;
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (mainCam != null)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * interactionDistance);
        }
    }
}
