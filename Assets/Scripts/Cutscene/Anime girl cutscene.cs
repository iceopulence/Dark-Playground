using UnityEngine;
using System.Collections;
public class Animegirlcutscene : MonoBehaviour
{
    // Audio Components
    [Header("Audio Components")]
    [SerializeField] AudioEnvelopeSpeaking animeGirlVoice;

    // Animator Components
    [Header("Animator Components")]
    [SerializeField] Animator playerAnimator;

    // Transform Components
    [Header("Transform Components")]
    [SerializeField] Transform animeGirl;
    [SerializeField] Transform playerStartPlacement;
    Transform playerT;

    // Camera Components
    [Header("Camera Settings")]
    [SerializeField] Camera mainCamera;
    [Tooltip("Secondary camera used for specific sequences.")]
    public Camera cutsceneCamera;
    [Tooltip("Animated camera component attached to the secondary camera.")]
    public AnimatedCamera animatedCamera;
    [Header("Anime Girl Targets")]
    public Transform agTarget1;

    [Header("Camera Targets")]

    public Transform cTarget1;

    [Header("Player Targets")]
    public Transform pTarget1;

    // Player Components
    [Header("Player Components")]
    [SerializeField] ThirdPersonController playerController;
    void Awake()
    {
        playerT = GameManager.Instance.playerT;
        playerController = GameManager.Instance.playerController;
        playerAnimator = GameManager.Instance.playerAnimator;

        mainCamera = Camera.main;
        animeGirlVoice = animeGirl.gameObject.GetComponent<AudioEnvelopeSpeaking>();

        animatedCamera = cutsceneCamera.gameObject.GetComponent<AnimatedCamera>();
    }
    public void StartCutscene()
    {
        StartCoroutine(CutScene());
    }
    public void EndCutscene()
    {

    }

    IEnumerator CutScene()
    {
        // mainCamera.gameObject.SetActive(false);
        // cutsceneCamera.gameObject.SetActive(true);

        // playerT.position = playerStartPlacement.position;
        // playerT.rotation = playerStartPlacement.rotation;
        // playerController.DeactivateControls();
        // animatedCamera.SetTargetTransform(cTarget1);


        // float startTime = Time.time;
        // Vector3 startPosition = playerT.position;
        // float speed = 0f;
        // while (Time.time < startTime + moveDuration)
        // {
        //     Vector3 previousPosition = playerT.position;
        //     playerT.position = Vector3.Lerp(startPosition, destination.position, (Time.time - startTime) / moveDuration);
        //     speed = Vector3.Distance(playerT.position, previousPosition) / Time.deltaTime;
        //     playerAnimator.SetFloat("speedZ", speed);
        //     yield return null;
        // }

        // playerAnimator.SetFloat("speedZ", 0);

        // // player turns around to look at dandid
        // Quaternion initialRotation = animeGirl.rotation;
        // Quaternion targetRotation = Quaternion.Euler(0, initialRotation.eulerAngles.y + 180, 0);
        // float rotationSpeed = 1f; // Speed of rotation
        // float rotationProgress = 0f; // Progress from 0 to 1
        // while (rotationProgress < 1f)
        // {
        //     rotationProgress += Time.deltaTime * rotationSpeed;
        //     animeGirl.rotation = Quaternion.Lerp(initialRotation, targetRotation, rotationProgress);
        //     yield return null;
        // }
    yield return null;


    }
}
