using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Variables")]
    public GameObject player;
    public Transform playerT { get; private set; }
    ThirdPersonController playerController;
    CharacterController playerCC;
    PlayerInteraction playerInteraction;
    public AnimationController playerAnimController;

    [SerializeField] CinemachineBrain cinemachineBrain;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    [Space]
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] TextMeshProUGUI objectiveText;
    private const float nonActiveAlphaValue = 0.7f;
    public float textFadeOutSpeed = 0.5f;
    public AudioSource audioSource { get; private set; }

    [SerializeField] public ScreenFader screenFader;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        InitPlayer();
        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;

        print("ran awake on GameManager");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitPlayer();
        screenFader.FadeIn(3);
        print("ran on sceneloaded on GameManager");
    }

    void InitPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (player != null)
        {
            playerCC = !playerCC ? player.GetComponent<CharacterController>() : playerCC;
            playerController = !playerController ? player.GetComponent<ThirdPersonController>() : playerController;
            playerInteraction = !playerInteraction ? player.GetComponent<PlayerInteraction>() : playerInteraction;
            playerAnimController = !playerAnimController ? player.GetComponent<AnimationController>() : playerAnimController;
            playerT = player.transform;
        }
        Transform levelSpawn = GameObject.FindWithTag("Respawn").transform;
        TeleportPlayer(levelSpawn.position, levelSpawn.rotation);
        playerController.movementEnabled = true;

        // Initialize screenFader if it's null
        if (screenFader == null)
        {
            GameObject screenFaderObj = GameObject.FindWithTag("Screen Fade");
            if (screenFaderObj != null)
                screenFader = screenFaderObj.GetComponent<ScreenFader>();
        }

        if (objectiveText == null)
        {
            GameObject.FindWithTag("Objective Text");
        }
    }

    void Update()
    {
        if (objectiveText.alpha > nonActiveAlphaValue)
        {
            objectiveText.alpha -= Time.deltaTime * textFadeOutSpeed;
            if (objectiveText.alpha < nonActiveAlphaValue)
            {
                objectiveText.alpha = nonActiveAlphaValue;
            }
        }
    }

    public void UpdateObjectiveText(string text)
    {
        print("tried to update objective to " + text);
        objectiveText.text = text;
        objectiveText.alpha = 1f;
    }

    public void TeleportPlayer(Vector3 newPos, Quaternion newRot)
    {

        // Notify the virtual camera about the player's new position
        virtualCamera = GameObject.FindWithTag("annoying thing").GetComponent<CinemachineVirtualCamera>();
        virtualCamera.OnTargetObjectWarped(playerT, newPos - playerT.position);
        virtualCamera.ForceCameraPosition(newPos, newRot);
        virtualCamera.PreviousStateIsValid = false;

        // Disable character controller for teleportation
        playerCC.enabled = false;

        // Teleport the player to the skip location
        playerCC.transform.position = newPos;
        playerCC.transform.rotation = newRot;



        // Re-enable character controller
        playerCC.enabled = true;
    }

    // public void SetPlayerControlsEnabled(bool enabled)
    // {
    //     FirstPersonController.PlayerState state = enabled ? FirstPersonController.PlayerState.Movement : FirstPersonController.PlayerState.Disabled;

    //     Player.Instance.SetState(state);

    //     if(playerCC != null){playerCC.enabled = enabled;}
    //     if(playerInteraction != null){playerInteraction.enabled = enabled;}
    // }

    // public void SetPauseEnabled(bool enabled)
    // {
    //     pauseMenu.SetPauseEnabled(enabled);
    // }

}
