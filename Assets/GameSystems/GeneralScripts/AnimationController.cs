using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Transform handTransform; // Reference to the hand transform
    
    // Public method to place an object in the character's hand
    public void PlaceObjectInHand(Transform otherObject)
    {
        if (handTransform == null || otherObject == null)
        {
            Debug.LogWarning("Hand transform or object to place is not set.");
            return;
        }

        // Set the parent of the object to the hand transform
        otherObject.transform.SetParent(handTransform);

        // Calculate offset and position the object relative to the hand
        Vector3 offset = otherObject.transform.position - handTransform.position;
        otherObject.transform.localPosition = offset;
    }
}
