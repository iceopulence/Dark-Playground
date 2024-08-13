using UnityEngine;

public class CollectibleQuestItem : QuestTrigger
{
    public Color gizmoColor = new Color(1f, 1f, 0, 0.5f);
    [SerializeField] public string itemId;  // Ensure this field is public

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerEvent();
            Destroy(gameObject);
        }
    }

    protected override void TriggerEvent()
    {
        EventManager.TriggerEvent(GameEventType.ItemCollected, itemId);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
