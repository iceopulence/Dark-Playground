using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
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

    public MainMenuCameraController cameraController;

    void Start()
    {
        OpenMain();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMain();
        }
    }

    public void PlayGame()
    {
        cameraController.SetTargetTransform(mainTarget.targetTransform, mainTarget.smoothTime);
        SceneManager.LoadScene(1);
    }   

    public void OpenMain()
    {
        cameraController.SetTargetTransform(mainTarget.targetTransform, mainTarget.smoothTime);
    }

    public void OpenSettings()
    {
        cameraController.SetTargetTransform(settingsTarget.targetTransform, settingsTarget.smoothTime);
    }

    public void ShowCredits()
    {
        cameraController.SetTargetTransform(creditsTarget.targetTransform, creditsTarget.smoothTime);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
