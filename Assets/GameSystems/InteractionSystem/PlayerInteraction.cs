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

    void Start()
    {
        if (interactionIndicator != null)
        {
            interactionIndicator.enabled = false;
        }
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
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
                if (interactableObject != null)
                {
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
