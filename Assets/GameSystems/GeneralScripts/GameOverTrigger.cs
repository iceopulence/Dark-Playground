using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    public LayerMask triggerLayers;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is in the specified layer mask
        if (((1 << other.gameObject.layer) & triggerLayers) != 0)
        {
            GameManager.Instance.GameOver();
        }
    }
}
