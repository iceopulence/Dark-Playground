using UnityEngine;

public class CollectibleQuestItem : MonoBehaviour
{
    [SerializeField] public string itemId;  // Ensure this field is public

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.TriggerEvent(GameEventType.ItemCollected, itemId);
            Destroy(gameObject);
        }
    }
}
