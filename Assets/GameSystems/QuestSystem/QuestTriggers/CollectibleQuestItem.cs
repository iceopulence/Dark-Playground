using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollectibleQuestItem : QuestTrigger
{
    public Color gizmoColor = new Color(1f, 1f, 0, 0.5f);

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TriggerEvent();
            Complete();
        }
    }

    protected override void TriggerEvent()
    {
        EventManager.TriggerEvent(GameEventType.ItemCollected, targetId);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
