using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [SerializeField] private AudioSource footStepAudioSource;

    [SerializeField] private LayerMask groundMask;

    private FootStepData currentFootStepData;
    private RaycastHit lastRaycastHit;

    void Awake()
    {
        if (footStepAudioSource == null)
        {
            footStepAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void FixedUpdate()
    {
        GatherFootStepData();
    }

    private void GatherFootStepData()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 2f, groundMask))
        {
            lastRaycastHit = hit;
            GroundProperties groundProperties = hit.collider.GetComponent<GroundProperties>();
            if (groundProperties != null && currentFootStepData != groundProperties.GetNewFootStepData())
            {
                currentFootStepData = groundProperties.GetNewFootStepData();
            }
        }
    }

    public void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f && currentFootStepData != null && currentFootStepData.footstepSounds.Length > 0)
        {
            int index = Random.Range(0, currentFootStepData.footstepSounds.Length);
            float randomPitch = Random.Range(1 + currentFootStepData.pitchOffset - currentFootStepData.pitchRange, 
                                             1 + currentFootStepData.pitchOffset + currentFootStepData.pitchRange);
            footStepAudioSource.pitch = randomPitch;
            footStepAudioSource.PlayOneShot(currentFootStepData.footstepSounds[index], currentFootStepData.footstepVolume);
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f && currentFootStepData != null)
        {
            AudioSource.PlayClipAtPoint(currentFootStepData.footstepSounds[0], transform.position, currentFootStepData.footstepVolume);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 start = transform.position + Vector3.up * 0.5f;
        Vector3 end = start + Vector3.down * 2f;
        Gizmos.DrawLine(start, end);

        if (lastRaycastHit.collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(lastRaycastHit.point, 0.1f);
        }
    }
}
