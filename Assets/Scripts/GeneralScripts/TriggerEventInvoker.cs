using UnityEngine;
using UnityEngine.Events;

public class TriggerEventInvoker : MonoBehaviour
{
    // Public UnityEvent, can be assigned from the Unity Inspector
    public UnityEvent onTriggerEnterEvent;

    // LayerMask to filter which objects can trigger the event
    public LayerMask triggerLayers;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is in the specified layer mask
        if (((1 << other.gameObject.layer) & triggerLayers) != 0)
        {
            // Invoke the public event
            onTriggerEnterEvent.Invoke();
        }
    }
}
