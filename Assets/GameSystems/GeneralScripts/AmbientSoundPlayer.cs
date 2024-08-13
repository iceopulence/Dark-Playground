using System.Collections;
using UnityEngine;

public class AmbientSoundPlayer : MonoBehaviour
{
    public Transform player;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public float maxHeight = 5.0f;
    public float minRadius = 10f; // Minimum radius for sound playback
    public float maxRadius = 70f; // Maximum radius for sound playback
    public float minWaitTime = 1.0f;
    public float maxWaitTime = 10.0f;
    public bool isRandomSegmentMode = false; // Switch between modes
    public float minDuration = 1.0f; // Minimum duration for a segment
    public float maxDuration = 5.0f; // Maximum duration for a segment

    void Start()
    {
        StartCoroutine(PlayAmbientSounds());
    }

    IEnumerator PlayAmbientSounds()
    {
        while (true)
        {
            if (!isRandomSegmentMode)
            {
                // Mode 1: Play a random clip in full
                MoveAndPlayFullClip();
            }
            else
            {
                // Mode 2: Play a random segment from the first clip
                PlayRandomSegmentFromFirstClip();
            }

            // Wait for a random time interval
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void MoveAndPlayFullClip()
    {
        Vector3 newPosition = GetRandomPosition();
        audioSource.transform.position = newPosition;
        AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayRandomSegmentFromFirstClip()
    {
        Vector3 newPosition = GetRandomPosition();
        audioSource.transform.position = newPosition;
        AudioClip clip = audioClips[0]; // Always use the first clip in the array
        audioSource.clip = clip;

        float duration = Random.Range(minDuration, Mathf.Min(maxDuration, clip.length));
        float startTime = Random.Range(0, clip.length - duration);

        audioSource.time = startTime;
        audioSource.Play();
        audioSource.SetScheduledEndTime(AudioSettings.dspTime + duration);
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized * Random.Range(minRadius, maxRadius);
        randomDirection.y = 0; // Keep sound on the same Y level or a bit higher
        float randomHeight = Random.Range(0, maxHeight);
        return player.position + randomDirection + Vector3.up * randomHeight;
    }

    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            // Draw the minimum radius
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, minRadius);

            // Draw the maximum radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, maxRadius);

            // Draw a vertical line to represent the maximum height
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(player.position, new Vector3(player.position.x, player.position.y + maxHeight, player.position.z));
        }
    }
}
