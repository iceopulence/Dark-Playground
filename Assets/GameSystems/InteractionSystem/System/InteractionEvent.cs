using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour, IInteractable
{
    public UnityEvent interactionEvent;
    
    public void OnInteract(PlayerInteraction interactor)
    {
        interactionEvent.Invoke();
    }
}
