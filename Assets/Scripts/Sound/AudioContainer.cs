using UnityEngine;

[CreateAssetMenu(fileName = "AudioContainer", menuName = "GameData/Audio/AudioContainer")]
public class AudioContainer : ScriptableObject
{
    public AudioClip[] hitSounds;
    public AudioClip deathSound;
    
    public AudioClip[] slidingSounds;
    public AudioClip[] bumpSounds;
}
