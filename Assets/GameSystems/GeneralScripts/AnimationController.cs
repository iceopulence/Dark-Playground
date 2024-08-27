using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Transform handAttachPoint; // Reference to the hand transform

    Animator animator;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Public method to place an object in the character's hand
    public void PlaceObjectInHand(Transform otherObject)
    {
        if (handAttachPoint == null || otherObject == null)
        {
            Debug.LogWarning("Hand transform or object to place is not set.");
            return;
        }

        Rigidbody objRb = otherObject.GetComponent<Rigidbody>();
        if (objRb != null)
        {
            objRb.isKinematic = true;
        }

        // Set the parent of the object to the hand transform
        otherObject.transform.SetParent(handAttachPoint);

        // Calculate offset and position the object relative to the hand
        Vector3 offset = otherObject.transform.position - handAttachPoint.position;
        otherObject.transform.position = handAttachPoint.position + offset;
    }

    public void DropHeldObject()
    {
        // Iterate through all children of the handAttachPoint and destroy them
        foreach (Transform child in handAttachPoint)
        {
            if (child != handAttachPoint && !child.gameObject.CompareTag("Player"))
            {
                Rigidbody childRb = child.GetComponent<Rigidbody>();


                if (childRb != null)
                {
                    childRb.isKinematic = false;
                    child.SetParent(null);

                }
            }

        }
    }

    public void TakeOutObject()
    {
        ClearHands();

        animator.SetTrigger("Take Out Item");


    }

    public void SpawnItemIntoHand()
    {

        GameObject itemToSpawn = InventoryManager.Instance.heldItem.itemPrefab;
        Instantiate(itemToSpawn, handAttachPoint.position, itemToSpawn.transform.rotation, handAttachPoint);
    }

    public void ClearHands()
    {
        // Iterate through all children of the handAttachPoint and destroy them
        foreach (Transform child in handAttachPoint)
        {
            if (child != handAttachPoint && !child.gameObject.CompareTag("Player"))
            {
                print(child.gameObject.name + " in foreach");
                Destroy(child.gameObject);
            }

        }
    }


}
