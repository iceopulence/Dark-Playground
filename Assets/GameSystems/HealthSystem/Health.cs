using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    private float currentHealth;

    public UnityEvent onDeath;

    public AudioClip[] hitSounds;

    public GameObject droppedItem;

    void Start()
    {
        // Initialize health to maximum at the start.
        currentHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        // Reduce health by damage amount and clamp it to 0 at minimum.
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        print("damaged " + currentHealth);

        if(hitSounds != null)
        {
            AudioSource.PlayClipAtPoint(hitSounds[Random.Range(0, hitSounds.Length)], transform.position);
        }

        // Check if health has dropped to zero or below.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        // Increase health by the specified amount and clamp it to the maximum health.
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    private void Die()
    {
        // Handle what happens when health is depleted.
        Debug.Log(gameObject.name + " has died.");
        // Optionally deactivate the game object.
        // gameObject.SetActive(false);

        Instantiate(droppedItem, transform.position, transform.rotation);

        onDeath.Invoke();
    }
}
