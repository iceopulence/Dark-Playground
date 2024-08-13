using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PrisonEntranceCutscene : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondaryCamera;
    public ScreenFader screenFader;


    public Animator doorAnimator;
    public Transform movingObject;
    public Transform startingPoint;
    public Transform destination;
    public Animator playerAnimator; // Animator for the player
    public ThirdPersonController controller; // Reference to the ThirdPersonController

    public float waitTimeBeforeAnimation = 3f;
    public float waitTimeAfterAnimation = 2f;
    public float moveDuration = 3f;

    [Header("Door Sounds")]
    public AudioSource doorAudioSource;
    public AudioClip doorOpenSFX;
    public AudioClip doorCloseSFX;
    public AudioClip doorSlamSFX;
    public AudioClip doorHandleSFX;
    public AudioClip billieJean;


    void Awake()
    {
        if (screenFader == null)
        {
            GameObject screenFaderObj = GameObject.FindWithTag("Screen Fade");
            if (screenFaderObj != null)
                screenFader = screenFaderObj.GetComponent<ScreenFader>();
        }
        mainCamera = mainCamera != null ? mainCamera : Camera.main;

        secondaryCamera.gameObject.SetActive(false);
    }

    public void StartCutscene()
    {
        StartCoroutine(ActionSequence());
    }

    private IEnumerator ActionSequence()
    {
        // Disable player controls
        controller.DeactivateControls();

        screenFader.FadeToBlack(2);
        yield return new WaitWhile(() => screenFader.isFading);

        // Turn off the main camera and turn on the secondary camera
        mainCamera.gameObject.SetActive(false);
        secondaryCamera.gameObject.SetActive(true);
        movingObject.position = startingPoint.position;
        movingObject.rotation = startingPoint.rotation;

        if (playerAnimator.GetBool("air"))
        {
            movingObject.eulerAngles = new Vector3(movingObject.eulerAngles.x, movingObject.eulerAngles.y + 180, movingObject.eulerAngles.z);
            AudioSource.PlayClipAtPoint(billieJean, movingObject.position);
        }
        
        yield return new WaitForSeconds(0.2f);
        screenFader.FadeIn(1);
        yield return new WaitWhile(() => screenFader.isFading);

        // Wait for a few seconds before starting the animation
        yield return new WaitForSeconds(waitTimeBeforeAnimation);

        // Start the door opening animation
        doorAnimator.SetBool("doorOpen", true);

        //made it quicker to put here
        doorAudioSource.PlayOneShot(doorHandleSFX);

        // Wait for a couple more seconds before moving the object
        yield return new WaitForSeconds(5f);

        // Lerp the position of the object to the destination
        float startTime = Time.time;
        Vector3 startPosition = movingObject.position;
        float speed = 0f;
        while (Time.time < startTime + moveDuration)
        {
            Vector3 previousPosition = movingObject.position;
            movingObject.position = Vector3.Lerp(startPosition, destination.position, (Time.time - startTime) / moveDuration);
            speed = Vector3.Distance(movingObject.position, previousPosition) / Time.deltaTime;
            playerAnimator.SetFloat("speedZ", speed);
            yield return null;
        }

        playerAnimator.SetFloat("speedZ", 0);


        // Set the animator's door open boolean to false
        doorAnimator.SetBool("doorOpen", false);
        doorAudioSource.PlayOneShot(doorCloseSFX);

        // Wait a few seconds before ending the coroutine
        yield return new WaitForSeconds(waitTimeAfterAnimation);

        //fade to black
        screenFader.FadeToBlack(waitTimeAfterAnimation);
        yield return new WaitWhile(() => screenFader.isFading);

        // load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //handled by anim event
    public void PlayDoorSlam()
    {
        doorAudioSource.PlayOneShot(doorSlamSFX);
    }
    //handled by anim event
    public void PlayDoorOpen()
    {
        doorAudioSource.PlayOneShot(doorOpenSFX);
    }
}
