using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class FirstDandidEncounterController : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("Sound clip to be played during sequences.")]
    public AudioClip soundClip;

    [Header("Timing Settings")]
    [Tooltip("Time to wait after playing the sound.")]
    public float waitTime = 1f;

    [Header("Dandid Settings")]
    [Tooltip("Animator state index for Dandid's animation.")]
    public int dandidAnimIndex = 0;
    [Tooltip("Transform component of Dandid.")]
    public Transform dandidTransform;
    [Tooltip("Move speed of Dandid.")]
    public float moveSpeed = 5f;

    [Header("Sequence Targets")]
    [Tooltip("Initial target for Dandid's movement.")]
    public Transform moveTarget;
    [Tooltip("Secondary target for Dandid's movement during the second sequence.")]
    public Transform secondaryMoveTarget;
    [Tooltip("Final target for Dandid's movement during the second sequence.")]
    public Transform finalMoveTarget;

    [Header("Player Settings")]
    [Tooltip("Player's starting placement for the second sequence.")]
    public Transform playerStartPlacement2ndSequence;
    Transform playerTransform;
    ThirdPersonController playerController;
    public Transform playerHandT, soapT;
    public AudioClip playerHurtSound;
    public AudioClip powderSFX;
    public AudioClip transitioningSound;
    

    [Header("Camera Settings")]
    Camera mainCamera;
    [Tooltip("Secondary camera used for specific sequences.")]
    public Camera secondaryCamera;
    [Tooltip("Animated camera component attached to the secondary camera.")]
    public AnimatedCamera animatedCamera;
    public Transform cameraSecondTarget;

    ScreenFader screenFader;

    [Header("Events")]
    [Tooltip("Event triggered at the start of the first sequence.")]
    public UnityEvent onStartFirstSequence;
    [Tooltip("Event triggered at the start of the second sequence.")]
    public UnityEvent onStartSecondSequence;

    private Animator dandidAnimator;
    private Animator playerAnimator;

    void Start()
    {
        playerTransform = GameManager.Instance.player.transform;

        dandidAnimator = dandidTransform.GetComponent<Animator>();
        playerAnimator = playerTransform.GetComponent<Animator>();

        playerController = playerTransform.GetComponent<ThirdPersonController>();
        mainCamera = Camera.main;
        
        screenFader = GameManager.Instance.screenFader;
    }

    public void StartSequence()
    {
        onStartFirstSequence.Invoke();
        StartCoroutine(PlaySoundAndAnimate());
    }

    private IEnumerator PlaySoundAndAnimate()
    {
        AudioSource.PlayClipAtPoint(soundClip, dandidTransform.position);
        yield return new WaitForSeconds(waitTime);

        if (dandidAnimator != null)
        {
            dandidAnimator.SetInteger("AnimState", dandidAnimIndex);
        }

        yield return MoveDandid(moveTarget, 2.5f);
        dandidTransform.gameObject.SetActive(false);
    }

    public void StartSequence2()
    {
        onStartSecondSequence.Invoke();
        StartCoroutine(Sequence2());
    }

    private IEnumerator Sequence2()
    {
        playerController.DeactivateControls();

        playerTransform.position = playerStartPlacement2ndSequence.transform.position;
        playerTransform.rotation = playerStartPlacement2ndSequence.transform.rotation;

        mainCamera.gameObject.SetActive(false);
        secondaryCamera.gameObject.SetActive(true);

        playerAnimator.SetTrigger("Pickup Soap");
        float timeToPickupSoap = 2.1f;
        yield return new WaitForSeconds(timeToPickupSoap);
        GameManager.Instance.playerAnimController.PlaceObjectInHand(soapT);
        yield return new WaitForSeconds(4f - timeToPickupSoap); // Assuming duration of the animation is known

        animatedCamera.SetTargetTransform(cameraSecondTarget);
        yield return new WaitWhile(() => animatedCamera.isAnimating);

        // Rotate the player smoothly
        Quaternion initialRotation = playerTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, initialRotation.eulerAngles.y + 180, 0);
        float rotationSpeed = 1f; // Speed of rotation
        float rotationProgress = 0f; // Progress from 0 to 1

        dandidTransform.position = secondaryMoveTarget.position;
        dandidTransform.rotation = secondaryMoveTarget.rotation;
        dandidTransform.gameObject.SetActive(true);
        yield return MoveDandid(finalMoveTarget, 2f);

        while (rotationProgress < 1f)
        {
            rotationProgress += Time.deltaTime * rotationSpeed;
            playerTransform.rotation = Quaternion.Lerp(initialRotation, targetRotation, rotationProgress);
            yield return null;
        }

        dandidAnimator.SetTrigger("Sprinkle Powder");
        AudioSource.PlayClipAtPoint(powderSFX, dandidTransform.position);
        yield return new WaitForSeconds(1.5f);
        playerAnimator.Play("Fall Over");
        AudioSource.PlayClipAtPoint(playerHurtSound, playerTransform.position);

        //fade to black
        screenFader.FadeToBlack(5);
        yield return new WaitForSeconds(1.5f);
        AudioSource.PlayClipAtPoint(transitioningSound, secondaryCamera.transform.position, 0.2f);
        yield return new WaitWhile(() => screenFader.isFading);
        yield return new WaitForSeconds(0.5f);

        // load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        mainCamera.gameObject.SetActive(true);
    }

    //make use duration and progress later
    private IEnumerator MoveDandid(Transform target, float moveSpeed)
    {
        float step = moveSpeed * Time.deltaTime;
        while (Vector3.Distance(dandidTransform.position, target.position) > 0.01f)
        {
            dandidTransform.position = Vector3.MoveTowards(dandidTransform.position, target.position, step);
            yield return null;
        }
    }
}
