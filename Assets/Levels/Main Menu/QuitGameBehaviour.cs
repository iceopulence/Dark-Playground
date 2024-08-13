using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class QuitGameBehaviour : MonoBehaviour
{

    public GameObject areYouSureAboutThat;


    public MainMenuCameraController menuCameraController;

    [SerializeField] MainMenuManager mainMenuManager;

    public CameraTarget turnAroundTarget;

    public CameraTarget leaveTarget;

    public CameraTarget areYouSurePosition;

    public AudioSource aysAudioSource;
    public AudioSource footStepAudioSource;

    public AudioClip AYSVoiceline;

    public AudioClip footStepsSFX;
    public AudioClip altFootStepsSFX;

    public ScreenFader screenFader;

    public UnityEvent onDenyQuit;

    void Awake()
    {
        if(screenFader == null)
        {
            GameObject screenFaderObj = GameObject.FindWithTag("Screen Fade");
            if(screenFaderObj != null)
                screenFader = screenFaderObj.GetComponent<ScreenFader>();
        }
    }

    public void StartCinematic()
    {
        StartCoroutine(QuitCinematic());
    }

    IEnumerator QuitCinematic()
    {
        menuCameraController.SetTargetTransform(turnAroundTarget.transform);
        footStepAudioSource.clip = footStepsSFX;
        footStepAudioSource.Play();
        yield return null;
        yield return new WaitWhile(() => menuCameraController.isAnimating);

        menuCameraController.SetTargetTransform(areYouSurePosition.transform);
        yield return null;
        yield return new WaitWhile(() => menuCameraController.isAnimating);

        menuCameraController.SetTargetRotation(new Vector3(0f,193f,0f));
        AreYouSure();
        footStepAudioSource.Stop();
        yield return new WaitWhile(() => menuCameraController.isAnimating);
    }

    void AreYouSure()
    {
        areYouSureAboutThat.SetActive(true);
        if(aysAudioSource && AYSVoiceline)
        {
            aysAudioSource.clip = AYSVoiceline;
            aysAudioSource.Play();
        }
    }

    IEnumerator ConfirmQuitCoroutine()
    {
        menuCameraController.SetTargetTransform(leaveTarget.transform);
        screenFader.FadeToBlack(3);
        footStepAudioSource.clip = footStepsSFX;
        footStepAudioSource.Play();
        yield return new WaitWhile(() => screenFader.isFading);
        
        Application.Quit();
    }

    public void ConfirmQuit()
    {
        StartCoroutine(ConfirmQuitCoroutine());
    }


    public void DenyQuit()
    {
        footStepAudioSource.clip = altFootStepsSFX;
        footStepAudioSource.Play();
        menuCameraController.SetTargetTransform(mainMenuManager.mainTarget.transform);
    }
}