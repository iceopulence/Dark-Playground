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

    public AudioSource audioSource;

    AudioClip AYSVoiceline;

    public ScreenFader screenFader;

    public UnityEvent onDenyQuit;



    public void StartCinematic()
    {
        StartCoroutine(QuitCinematic());
    }

    IEnumerator QuitCinematic()
    {
        menuCameraController.SetTargetTransform(turnAroundTarget.transform);
        yield return null;
        yield return new WaitWhile(() => menuCameraController.isAnimating);

        menuCameraController.SetTargetTransform(areYouSurePosition.transform);
        yield return null;
        yield return new WaitWhile(() => menuCameraController.isAnimating);

        menuCameraController.SetTargetRotation(new Vector3(0f,193f,0f));
        AreYouSure();
        yield return new WaitWhile(() => menuCameraController.isAnimating);
    }

    void AreYouSure()
    {
        areYouSureAboutThat.SetActive(true);
        if(audioSource && AYSVoiceline)
        {
            audioSource.clip = AYSVoiceline;
            audioSource.Play();
        }
    }

    IEnumerator ConfirmQuitCoroutine()
    {
        menuCameraController.SetTargetTransform(leaveTarget.transform);
        screenFader.FadeToBlack(3);
        yield return new WaitWhile(() => screenFader.isFading);
        
        Application.Quit();
    }

    public void ConfirmQuit()
    {
        StartCoroutine(ConfirmQuitCoroutine());
    }


    public void DenyQuit()
    {
        menuCameraController.SetTargetTransform(mainMenuManager.mainTarget.transform);
    }
}