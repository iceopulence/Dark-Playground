using UnityEngine;

public class LocationTrigger : MonoBehaviour
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
            EventManager.TriggerEvent(GameEventType.LocationReached, transform.position);
            // Optionally, disable this script after the event is triggered
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
