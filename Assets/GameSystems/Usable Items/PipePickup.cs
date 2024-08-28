using UnityEngine;
using System.Collections;

public class PipePickup : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO itemSO;
    [SerializeField] MeshRenderer mr;

    [SerializeField] AudioClip pipeUnlockedVoiceLine;

    public void OnInteract(PlayerInteraction interactor)
    {
        if (interactor)
        {
            mr.enabled = false;

            WeaponHandler wh = interactor.GetComponent<WeaponHandler>();
            wh.hasPipe = true;

            StartCoroutine(PickupPipe());
        }
    }

    IEnumerator PickupPipe()
    {
        GameManager.Instance.playerVoiceLineController.PlayVoiceLine("a pipe");
        float lineDuration = GameManager.Instance.playerVoiceLineController.GetVoiceLineLength("a pipe");
        yield return new WaitForSeconds(lineDuration);
        GameManager.Instance.PlaySound(pipeUnlockedVoiceLine);
        Destroy(this.gameObject);
    }
}