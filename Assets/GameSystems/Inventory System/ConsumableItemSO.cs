using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class ConsumableItemSO : ItemSO
{
    public int healthRestore;

    public override void Use()
    {
        Debug.Log($"Consuming {itemName}, restoring {healthRestore} health.");
    }
}
