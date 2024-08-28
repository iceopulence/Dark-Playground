using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip[] doorSounds;
    [SerializeField] private AudioClip lockedSound; // Sound to play when the door is locked
    [SerializeField] private float openAngle = 90f;
    public float swingDuration = 0.75f;
    [SerializeField] private bool openDirectionPositive = true;
    [SerializeField] public string requiredKey = "MasterKey"; // The specific string value for the key to unlock the door
    public bool isOpen = false;
    public bool isLocked = true;
    public Door linkedDoor;
    private bool isSwinging = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine swingCoroutine;
    [SerializeField] private BoxCollider boxCollider; // Reference to the BoxCollider
    [SerializeField] private GameObject lockedChain;
    [SerializeField] private GameObject unlockedChain;

    private VideoPlayer videoPlayer;
    private ScreenFader screenFader;

    public enum HingeAxis { X, Y, Z }

    public HingeAxis swingAxis = HingeAxis.Z;

    private Coroutine unlockingCoroutine;

    void Awake()
    {
        if (doorSounds.Length == 0)
        {
            doorSounds = Resources.LoadAll<AudioClip>("sfx/DoorSounds/DoorOpening");
        }

        if (lockedSound == null)
        {
            lockedSound = Resources.Load<AudioClip>("sfx/DoorSounds/LockedDoor/door locked");
        }


        videoPlayer = GameManager.Instance.videoPlayer;
        screenFader = GameManager.Instance.screenFader;

        closedRotation = transform.rotation;
        float swingAngle = openDirectionPositive ? openAngle : -openAngle;
        openRotation = closedRotation * Quaternion.Euler(swingAxis == HingeAxis.X ? swingAngle : 0, swingAxis == HingeAxis.Y ? swingAngle : 0, swingAxis == HingeAxis.Z ? swingAngle : 0);
        boxCollider = GetComponent<BoxCollider>(); // Get the BoxCollider component

        if (isOpen)
        {
            swingCoroutine = StartCoroutine(SwingDoor(openRotation));
        }
        if (transform.parent != null && linkedDoor == null)
        {
            InitLinkedDoor();
        }

        // Make sure the door is on the right layer
        if (gameObject.layer != 8)
            gameObject.layer = 8;
    }

    public void OnInteract(PlayerInteraction interactor)
    {
        if (isSwinging)
        {
            StopCoroutine(swingCoroutine);
        }

        if (isLocked)
        {
            // Check if the player has the correct key
            if (PlayerHasKey(requiredKey))
            {
                if(unlockingCoroutine == null)
                {
                    GameManager.Instance.playerController.DeactivateControls();
                    unlockingCoroutine = StartCoroutine(UnlockDoorSequence());
                }
            }
            else
            {
                // Play locked door sound if locked
                if (lockedSound != null)
                {
                    AudioSource.PlayClipAtPoint(lockedSound, transform.position);
                }
                return; // Exit the method to prevent door from toggling open state
            }
        }
        else
        {
            ToggleDoorState();
        }
    }

    private void ToggleDoorState()
    {
        isOpen = !isOpen;
        swingCoroutine = StartCoroutine(SwingDoor(isOpen ? openRotation : closedRotation));

        if (doorSounds.Length > 0)
        {
            AudioClip doorSound = doorSounds[Random.Range(0, doorSounds.Length)];
            AudioSource.PlayClipAtPoint(doorSound, transform.position);
        }
    }

    private IEnumerator SwingDoor(Quaternion targetRotation)
    {
        isSwinging = true;
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < swingDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / swingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        isSwinging = false;
    }

    private bool PlayerHasKey(string key)
    {
        // Check if the player has the required key. Implement your own logic here
        return InventoryManager.Instance.CheckHasKey(key);
    }

    private IEnumerator UnlockDoorSequence()
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
        yield return new WaitWhile(() => !videoPlayer.isPlaying);  // Give the video player time to start

        // Wait for the video to finish playing
        yield return new WaitWhile(() => videoPlayer.isPlaying);

        videoPlayer.gameObject.SetActive(false);

        // Swap the chains
        if (lockedChain != null && unlockedChain != null)
        {
            lockedChain.SetActive(false);
            unlockedChain.SetActive(true);
        }

        // Unlock the door and the door next to it if its there
        isLocked = false;
        if (linkedDoor != null)
        {
            linkedDoor.isLocked = false;
        }

        // Fade back to the game scene
        screenFader.FadeIn(1);
        GameManager.Instance.playerController.ActivateControls();
        yield return new WaitWhile(() => screenFader.isFading);
        unlockingCoroutine = null;
    }

    void InitLinkedDoor()
    {
        Door[] doorSiblings = transform.parent.GetComponentsInChildren<Door>();
        foreach (Door d in doorSiblings)
        {
            if (d != this)
            {
                linkedDoor = d;
            }
        }
    }
}
