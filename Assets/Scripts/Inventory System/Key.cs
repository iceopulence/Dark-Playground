using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyID;

    void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
       rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayKeySound();
            InventoryManager.Instance.AddKey(keyID);
            Destroy(this.gameObject);
        }
    }

    void PlayKeySound()
    {
        AudioClip[] keyPickupSounds = Resources.LoadAll<AudioClip>("sfx/Key Sounds");
        print( keyPickupSounds.Length);
        if(keyPickupSounds.Length > 0)
        {
            AudioClip randomSFX = keyPickupSounds[Random.Range(0, keyPickupSounds.Length)];
            GameManager.Instance.PlaySound(randomSFX);
        }
    }
}
