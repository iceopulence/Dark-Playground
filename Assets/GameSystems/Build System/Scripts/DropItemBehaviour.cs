using UnityEngine;
using UnityEngine.UIElements;

public class DropItemOnDeath : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public float upForce = 10;
    public float sideForceMax = 5;

    DropInfo dropInfo;

    void Awake()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ApplyRandomForce();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Inventory inv = other.transform.GetComponent<Inventory>();
            inv.AddItem(dropInfo);
            Destroy(this.gameObject);
        }
    }

    void ApplyRandomForce()
    {
        Vector3 theforce = new Vector3(Random.Range(-sideForceMax, sideForceMax), upForce, Random.Range(-sideForceMax, sideForceMax));
        rb.AddForce(theforce);
    }
}
