using UnityEngine;

[CreateAssetMenu(fileName = "FootStepData", menuName = "GameData/Audio/FootStepData")]
public class FootStepData : ScriptableObject
{
    public AudioClip[] footstepSounds;
    public short groundTypeID = 0;
    public float footstepVolume = 0.3f;
    public float pitchOffset = 0;
    public float pitchRange = 0.15f;
}
