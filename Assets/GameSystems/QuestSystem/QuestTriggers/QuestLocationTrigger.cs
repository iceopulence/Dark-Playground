using UnityEngine;

public class QuestLocationTrigger : QuestTrigger
{
    private Transform playerT;
    [SerializeField] private float triggerRadius = 1f;

    void Start()
    {
        playerT = GameManager.Instance.playerT;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, playerT.position) <= triggerRadius)
        {
            TriggerEvent();
            Complete();
        }
    }

    protected override void TriggerEvent()
    {
        EventManager.TriggerEvent(GameEventType.LocationReached, transform.position);
    }

    void OnDrawGizmos()
    {
        Color gizmoCol = Color.white;
        gizmoCol.a = 0.5f;
        Gizmos.DrawSphere(transform.position, triggerRadius);
    }
}
