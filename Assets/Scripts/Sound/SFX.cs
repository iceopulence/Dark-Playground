using UnityEngine;
using System.Collections;

public class SFX : MonoBehaviour
{
    public AudioContainer audioContainer;
    private Rigidbody rb;
    private bool isPlayingSlidingSound = false; // Debounce flag

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionStay(Collision collision)
    {
        print(rb.linearVelocity.magnitude + " " + rb.angularVelocity.magnitude);
        if (rb.linearVelocity.magnitude > 0.05f && rb.angularVelocity.magnitude < 0.5f && !isPlayingSlidingSound)
        {
            StartCoroutine(PlaySlidingSound(collision.contacts[0].point));
        }
    }

    private IEnumerator PlaySlidingSound(Vector3 sfxPoint)
    {
        if (audioContainer.slidingSounds.Length > 0)
        {
            isPlayingSlidingSound = true; // Set debounce flag
            int randomInt = Random.Range(0, audioContainer.slidingSounds.Length);
            AudioClip selectedClip = audioContainer.slidingSounds[randomInt];
            AudioSource.PlayClipAtPoint(selectedClip, sfxPoint);

            yield return new WaitForSeconds(selectedClip.length); // Wait for the sound to finish

            isPlayingSlidingSound = false; // Reset debounce flag
        }
        else
        {
            Debug.LogWarning("Sliding sound was not assigned on " + audioContainer);
        }
    }
}
