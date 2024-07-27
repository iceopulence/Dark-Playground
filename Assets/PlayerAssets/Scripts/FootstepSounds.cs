using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] FootstepAudioClips;

    [SerializeField]
    private AudioClip LandingAudioClip;

    [SerializeField]
    private float FootstepAudioVolume = 0.5f;

    public void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f && FootstepAudioClips.Length > 0)
        {
            int index = Random.Range(0, FootstepAudioClips.Length);
            AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepAudioVolume);
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
