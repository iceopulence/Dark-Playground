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

    public ScreenFader fadeScreen;

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
        print("asdf");
        menuCameraController.SetTargetTransform(areYouSurePosition.transform);
        yield return null;
        yield return new WaitWhile(() => menuCameraController.isAnimating);
        print("asdf2");
        yield return null;
        menuCameraController.SetTargetRotation(Vector3.zero);
        AreYouSure();
        yield return null;
        yield return new WaitWhile(() => menuCameraController.isAnimating);
    }

    void AreYouSure()
    {
        if(audioSource && AYSVoiceline)
        {
            audioSource.clip = AYSVoiceline;
            areYouSureAboutThat.SetActive(true);
            audioSource.Play();
        }
    }

    IEnumerator ConfirmQuitCoroutine()
    {
        menuCameraController.SetTargetTransform(leaveTarget.transform);
        fadeScreen.FadeToBlack(3);
        yield return new WaitWhile(() => fadeScreen.isFading);
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