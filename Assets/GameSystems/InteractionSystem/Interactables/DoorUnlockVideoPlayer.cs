using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class DoorUnlockVideoPlayer : MonoBehaviour
{
    [SerializeField] GameObject lockedChain; 
    [SerializeField] GameObject unlockedChain;

    private VideoPlayer videoPlayer;
    private ScreenFader screenFader;

    private void Start()
    {
        // Setup the VideoPlayer component
        videoPlayer = GameManager.Instance.videoPlayer;
        screenFader = GameManager.Instance.screenFader;
    }

    public void UnlockDoor()
    {
        // Start the sequence to unlock the door
        StartCoroutine(UnlockSequence());
    }

    private IEnumerator UnlockSequence()
    {
        // Load the video clip from Resources
        VideoClip unlockVideoClip = Resources.Load<VideoClip>("Videos/UnlockDoorClip");
        if (unlockVideoClip == null)
        {
            Debug.LogError("Video clip not found at path: Resources/Videos/UnlockDoorClip");
            yield break;
        }

        // Set up the VideoPlayer with the loaded clip
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.clip = unlockVideoClip;

        // Fade to black using the GameManager's screenFader
        screenFader.FadeToBlack(1);
        yield return new WaitWhile(() => screenFader.isFading);

        // Play the video on the RenderTexture
        videoPlayer.Play();
        yield return new WaitForSeconds(0.1f);  // Give the video player time to start

        // Wait for the video to finish playing
        yield return new WaitWhile(() => videoPlayer.isPlaying);

        videoPlayer.gameObject.SetActive(false);

        // Swap the chains
        if (lockedChain != null && unlockedChain != null)
        {
            lockedChain.SetActive(false);
            unlockedChain.SetActive(true);
        }

        // Fade back to the game scene
        screenFader.FadeIn(1);
        yield return new WaitWhile(() => screenFader.isFading);
    }
}
