using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class DropItemOnDeath : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public float upForce = 10;
    public float sideForceMax = 5;

    [SerializeField] ItemSO itemSO;

    LayerMask playerLayer;

    public bool pickupOnTriggerEnter = false;

    void Awake()
    {
        transform.SetParent(null); // remove itself from the parent before getting deleted

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        playerLayer = LayerMask.NameToLayer("Player");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ApplyRandomForce();
    }


    void ApplyRandomForce()
    {
        Vector3 theforce = new Vector3(Random.Range(-sideForceMax, sideForceMax), upForce, Random.Range(-sideForceMax, sideForceMax));
        rb.AddForce(theforce);
    }

    public void AddDropToInventory()
    {
        AudioSource.PlayClipAtPoint(itemSO.pickupSFX, transform.position);
        InventoryManager.Instance.AddItem(itemSO);
        Destroy(this.gameObject);
    }

    public void OnInteract(PlayerInteraction interactor)
    {
        AddDropToInventory();
    }

    void OnTriggerEnter(Collider other)
    {
        if (pickupOnTriggerEnter && ((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            AddDropToInventory();
        }
        else if(rb.isKinematic != true)
        {
            rb.isKinematic = true;
            StartCoroutine(OscilateUpAndDown());
        }
    }

    IEnumerator OscilateUpAndDown()
    {
        float amplitude = 0.25f;  // The maximum distance to move up and down from the starting position.
        float frequency = 4f;  // Speed of oscillation.
        Vector3 startPosition = transform.position + Vector3.up * amplitude;

        while (true)
        {
            // Calculate the new Y position using a sine wave.
            float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;  // Wait for the next frame.
        }
    }
}
