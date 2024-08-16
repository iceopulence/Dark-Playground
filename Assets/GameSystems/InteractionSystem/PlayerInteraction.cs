// PlayerInteraction.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3.0f;
    public float sphereRadius = 0.5f;
    public LayerMask interactableLayer;
    public KeyCode interactKey;
    [SerializeField] private Image interactionIndicator;
    [SerializeField] private TextMeshProUGUI interactionText;

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

        TurnOffIndicators();
    }

    void Update()
    {
        if (mainCam != null)
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.SphereCast(ray, sphereRadius, out hit, interactionDistance, interactableLayer, QueryTriggerInteraction.Collide))
            {
                ShowIndicators(hit.collider.gameObject.name);

                if (Input.GetKeyDown(interactKey))
                {
                    IInteractable interactableObject = hit.collider.GetComponent<IInteractable>();
                    // print("Im already tracer");
                    if (interactableObject != null)
                    {
                        // print("what about widdowmaker");
                        interactableObject.OnInteract(this);
                    }
                }
            }
            else
            {
                _lastHitTransform = null;
                TurnOffIndicators();
            }
        }
    }

    void ShowIndicators(string name_interactable)
    {
        if (interactionIndicator)
        {
            interactionIndicator.enabled = true;
        }
        interactionText.text = "Press " + interactKey.ToString() + " to interact with " + name_interactable;
    }

    void TurnOffIndicators()
    {
        if (interactionIndicator != null)
        {
            interactionIndicator.enabled = false;
        }
        if (interactionText != null)
        {
            interactionText.text = "";
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
