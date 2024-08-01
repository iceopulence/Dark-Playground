using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public CameraTarget mainTarget;
    public CameraTarget settingsTarget;
    public CameraTarget creditsTarget;
    public MainMenuCameraController cameraController;

    Coroutine playGameCoroutine;
    Coroutine openSettingsCoroutine;

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
        if (playGameCoroutine == null)
        {
            playGameCoroutine = StartCoroutine(PlayGameCoroutine());
        }
    }

    public void OpenMain()
    {
        cameraController.SetTargetTransform(mainTarget.targetTransform, mainTarget.smoothTime);
    }

    public void OpenSettings()
    {
        if (openSettingsCoroutine == null)
        {
            openSettingsCoroutine = StartCoroutine(OpenSettingsCoroutine());
        }
    }

    public void ShowCredits()
    {
        cameraController.SetTargetTransform(creditsTarget.targetTransform, creditsTarget.smoothTime);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    IEnumerator PlayGameCoroutine()
    {
        cameraController.SetTargetTransform(mainTarget.targetTransform, mainTarget.smoothTime);
        //fade to black subscibe to complete event
        yield return new WaitForSeconds(1);
        playGameCoroutine = null;
        SceneManager.LoadScene(1);
        
    }

    IEnumerator OpenSettingsCoroutine()
    {
        cameraController.SetTargetTransform(settingsTarget.targetTransform, settingsTarget.smoothTime);
        yield return null; //possibly implement animation afterwards

        openSettingsCoroutine = null;
    }
}


[System.Serializable]
public class CameraTarget
{
    public Transform targetTransform;
    public float smoothTime;
}