using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip[] doorSounds;

    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float swingDuration = 1f;
    [SerializeField] private bool openDirectionPositive = true;

    public bool isOpen = false;
    private bool isSwinging = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine swingCoroutine;

    private void Start()
    {
        // save the original closed rotation
        closedRotation = transform.rotation;
        // calculate the open rotation based on the opening direction
        openRotation = closedRotation * Quaternion.Euler(0, 0, openDirectionPositive ? openAngle : -openAngle);

        if(isOpen)
        {
            swingCoroutine = StartCoroutine(SwingDoor(openRotation));
        }
    }

    public void OnInteract()
    {
        if (isSwinging)
        {
            StopCoroutine(swingCoroutine);
        }
        print("Im already widdowmaker");
        isOpen = !isOpen;
        swingCoroutine = StartCoroutine(SwingDoor(isOpen ? openRotation : closedRotation));

        // play a random sound effect if the audio source is not null
        if (doorSounds.Length > 0)
        {
            AudioClip doorSound = doorSounds[Random.Range(0, doorSounds.Length)];
            AudioSource.PlayClipAtPoint(doorSound, transform.position);
        }
    }

    private IEnumerator SwingDoor(Quaternion targetRotation)
    {
        isSwinging = true;
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        print("I'll be bastion");

        while (elapsedTime < swingDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / swingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        print("Nerf bastion");
        isSwinging = false;
    }

    private void OnDrawGizmos()
    {
        // // draw the door's current state and intended open state
        // Gizmos.color = isOpen ? Color.green : Color.red;
        // Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        // Gizmos.DrawCube(Vector3.zero, new Vector3(1, 2, 0.1f));

        // if (!Application.isPlaying)
        // {
        //     // draw the open state when not playing
        //     Gizmos.matrix = Matrix4x4.TRS(transform.position, closedRotation * Quaternion.Euler(0, openDirectionPositive ? openAngle : -openAngle, 0), Vector3.one);
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawCube(Vector3.zero, new Vector3(1, 2, 0.1f));
        // }
    }
}
