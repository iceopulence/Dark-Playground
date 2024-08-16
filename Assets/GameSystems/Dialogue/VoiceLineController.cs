using System.Collections.Generic;
using UnityEngine;

public class VoiceLineController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] voiceClips;

    private Dictionary<string, AudioClip> voiceLineDictionary;

    private void Awake()
    {
        InitializeVoiceLines();
    }

    private void InitializeVoiceLines()
    {
        voiceLineDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in voiceClips)
        {
            if (clip != null && !voiceLineDictionary.ContainsKey(clip.name))
            {
                voiceLineDictionary.Add(clip.name, clip);
            }
        }
    }

    public void PlayVoiceLine(string clipName)
    {
        if (voiceLineDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Voice line with ID '{clipName}' not found.");
        }
    }

    public float GetVoiceLineLength(string clipName)
    {
        if (voiceLineDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            return clip.length;
        }
        else
        {
            Debug.LogWarning($"Voice line with ID '{clipName}' not found.");
            return 0;
        }
    }
}
