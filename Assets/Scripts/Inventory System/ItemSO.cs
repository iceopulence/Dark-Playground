using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject itemPrefab;  // GameObject for visualization in the game world
    public AudioClip pickupSFX;

    public abstract void Use();  // Define what happens when the item is used
    public virtual void OnPickup() => Debug.Log($"{itemName} picked up!");
    public virtual void OnDrop() => Debug.Log($"{itemName} dropped.");
}
