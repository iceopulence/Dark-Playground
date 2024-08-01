using UnityEngine;

public class QuitGameBehaviour : MonoBehaviour
{
    public GameObject areYouSureAboutThat;

    public MainMenuCameraController mainCameraController;
    [SerializeField] MainMenuManager mainMenuManager;

    public CameraTarget areYouSurePosition;
    public CameraTarget turnAroundTarget;
    public CameraTarget leaveTarget;

    //add a unity event variable for onDenyQuit
    //add the necessary namepace at the top
    //StartCenematic()
    //AreYouSure()
    //ConFirmQuit()
    //DenyQuit()
}
