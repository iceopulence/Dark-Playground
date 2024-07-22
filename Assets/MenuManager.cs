using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

    }

    public void ToggleSettings()
    {
        
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        paused = false;
        Time.timeScale = 1;
    }

    public void TogglePause()
    {
        if(paused) 
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        paused = true;
        Time.timeScale = 0;

    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Reset()
    {

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }



}