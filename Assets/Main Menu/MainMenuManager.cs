using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [System.Serializable]
    public class CameraTarget
    {
        public Transform targetTransform;
        public float smoothTime;
    }

    public CameraTarget mainTarget;
    public CameraTarget settingsTarget;
    public CameraTarget creditsTarget;
    public CameraTarget creditsTarget;
    public CameraTarget turnAroundTarget;
    public CameraTarget turnAroundTarget2;
    public CameraTarget leaveTarget;


    public MainMenuCameraController cameraController;

    void Start()
    {
        OpenMain();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMain();
        }
    }

    public void PlayGame()
    {
        StartCoroutine(PlayGame());
    }

    public void OpenMain()
    {
        StartCoroutine(OpenMain());
    }

    public void OpenSettings()
    {
        StartCoroutine(OpenSettings());
    }

    public void ShowCredits()
    {
        StartCoroutine(ShowCredits());
    }

    public void QuitGame()
    {
        StartCoroutine(QuitGame());
    }


    IEnumerator PlayGame()
    {
        cameraController.SetTargetTransform(mainTarget.targetTransform, mainTarget.smoothTime);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }

    IEnumerator OpenMain()
    {
        cameraController.SetTargetTransform(mainTarget.targetTransform, mainTarget.smoothTime);
    }

    IEnumerator OpenSettings()
    {
        cameraController.SetTargetTransform(settingsTarget.targetTransform, settingsTarget.smoothTime);
    }

    IEnumerator ShowCredits()
    {
        cameraController.SetTargetTransform(creditsTarget.targetTransform, creditsTarget.smoothTime);
    }

    IEnumerator QuitGame()
    {
        Application.Quit();
    }
}
