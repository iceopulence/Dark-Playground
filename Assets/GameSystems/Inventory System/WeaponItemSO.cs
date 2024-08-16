using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class WeaponItemSO : ItemSO
{
    public int damage;
    public float attackSpeed;

    public override void Use()
    {
        Debug.Log($"Using weapon: {itemName} with damage {damage}");
    }
}

