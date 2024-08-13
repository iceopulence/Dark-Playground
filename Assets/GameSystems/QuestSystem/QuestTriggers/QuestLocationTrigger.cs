using UnityEngine;
using UnityEngine.Events;

public class QuestLocationTrigger : MonoBehaviour
{
    private Transform playerT;
    [SerializeField] private float triggerRadius = 1f;

    public UnityEvent onCompletion;

    void Start()
    {
        playerT = GameManager.Instance.playerT;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, playerT.position) <= triggerRadius)
        {
            EventManager.TriggerEvent(GameEventType.LocationReached, transform.position);
            onCompletion.Invoke();

            enabled = false;
        }
    }

    void OnDrawGizmos()
    {
        Color gizmoCol = Color.white;
        gizmoCol.a = 0.5f;

        Gizmos.DrawSphere(transform.position, triggerRadius);
    }
}
