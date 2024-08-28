using UnityEngine;

public class DrawerMover : MonoBehaviour, IInteractable
{
    [SerializeField] private float moveDistance = 2.5f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isOpen = false;
    private float startTime;

    void Awake()
    {
        startPosition = transform.position;
        targetPosition = startPosition + -transform.up * moveDistance;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            float t = (Time.time - startTime) / moveDuration;
            transform.position = Vector3.Lerp(isOpen ? targetPosition : startPosition, isOpen ? startPosition : targetPosition, t);

            if (t > 1.0f)
            {
                isMoving = false;
                isOpen = !isOpen;
            }
        }
    }

    public void OnInteract(PlayerInteraction interactor)
    {
        StartMoving();
    }

    private void StartMoving()
    {
        if (!isMoving)
        {
            isMoving = true;
            startTime = Time.time;

            AudioClip moveClip = isOpen ? closeSound : openSound;
            AudioSource.PlayClipAtPoint(moveClip, transform.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition, targetPosition);
    }
}
