using UnityEngine;
using System.Collections;

public class KidneyRoomIntroCutscene : MonoBehaviour
{

    public bool testing;
    Animator playerAnimator;

    ThirdPersonController playerController;


    private Coroutine CutsceneCoroutine;

    [SerializeField] AudioClip introNoise;


    void Start()
    {
        playerAnimator = GameManager.Instance.playerAnimator;
        playerController = GameManager.Instance.playerController;


        if (!testing)
        {
            StartIntroCutscene();
        }

    }
    public void StartIntroCutscene()
    {
        CutsceneCoroutine = StartCoroutine(IntroCutscene());
        StartCoroutine(CheckForSkip());
    }

    IEnumerator IntroCutscene()
    {
        playerController.DeactivateControls();
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("Having a bad time", true);
        }
        else
        {
            Debug.Log("no animator ");
        }
        GameManager.Instance.PlaySound(introNoise);
        yield return new WaitForSeconds(introNoise.length);

        playerAnimator.SetBool("Having a bad time", false);

        yield return new WaitForSeconds(13f * 0.5f);

        VoiceLineController playerVoiceLineController = GameManager.Instance.playerVoiceLineController;

        playerVoiceLineController.PlayVoiceLine("where am I");
        yield return new WaitForSeconds(playerVoiceLineController.GetVoiceLineLength("where am I"));

        playerAnimator.SetTrigger("sadge");
        yield return new WaitForSeconds(1);
        playerVoiceLineController.PlayVoiceLine("where did my kidney goo");
        yield return new WaitForSeconds(playerVoiceLineController.GetVoiceLineLength("where did my kidney goo"));

        EndCutscene();
    }

    void EndCutscene()
    {
        playerAnimator.SetTrigger("resetAnim");
        playerController.ActivateControls();
    }
    IEnumerator CheckForSkip()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //skip
                StopCoroutine(CutsceneCoroutine);
                EndCutscene();
            }
            //wait for end of frame to sync with update and not crash the game
            yield return null;
        }



    }


}
