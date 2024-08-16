using UnityEngine;

[CreateAssetMenu(fileName = "New UtilityItem", menuName = "Inventory/Utility")]
public class UtilityItemSO : ItemSO
{
    public override void Use()
    {
        Debug.Log($"Using {itemName}.");
    }
}
