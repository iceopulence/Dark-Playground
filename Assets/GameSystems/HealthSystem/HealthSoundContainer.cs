using UnityEngine;

[CreateAssetMenu(fileName = "HealthSounds", menuName = "GameData/Audio/HealthSoundContainer")]
public class HealthSoundContainer : ScriptableObject
{
    public AudioClip[] hitSounds;
    public AudioClip deathSound;
}
