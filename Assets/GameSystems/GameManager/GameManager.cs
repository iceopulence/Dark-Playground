using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Space]
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] TextMeshProUGUI objectiveText;
    private const float nonActiveAlphaValue = 0.7f;
    public float textFadeOutSpeed = 0.5f;
    public AudioSource audioSource { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Instance.player = this.player;
            Destroy(this.gameObject);
        }

        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        audioSource = GetComponent<AudioSource>();
        
        playerCC = player.GetComponent<CharacterController>();
        playerController = player.GetComponent<ThirdPersonController>();
        playerInteraction = player.GetComponent<PlayerInteraction>();
        playerT = player.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(objectiveText.alpha > nonActiveAlphaValue)
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