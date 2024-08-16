using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class KidneyRoomManager : MonoBehaviour
{
    public static KidneyRoomManager Instance { get; private set; }

    int kidneyRoomIndex = -1;

    bool hasHappenedOnce = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Makes the object persistent across scenes
            kidneyRoomIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);  // Destroys the new object if an instance already exists
        }
    }

    void Start()
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex != kidneyRoomIndex)
        {
            Destroy(this.gameObject);
        }
    }
}
