using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] FootstepAudioClips;

    [SerializeField]
    private AudioClip LandingAudioClip;

    [SerializeField]
    private float FootstepAudioVolume = 0.5f;

    [Header("AudioSource Settings")]
    [SerializeField] AudioSource footStepAudioSource;
    public float pitchRange = 0.25f;
    public float pitchOffset = 0;
    bool useOwnSource = false;

    void Awake()
    {
        useOwnSource = footStepAudioSource != null;
    }

    public void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f && FootstepAudioClips.Length > 0)
        {
            int index = Random.Range(0, FootstepAudioClips.Length);

            if(useOwnSource)
            {
                float randomPitch = Random.Range(1 + pitchOffset - pitchRange, 1 + pitchOffset + pitchRange);
                footStepAudioSource.pitch = randomPitch;
                footStepAudioSource.PlayOneShot(FootstepAudioClips[index], FootstepAudioVolume);
            }
            else
            {
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.position, FootstepAudioVolume);
        }
    }
}
