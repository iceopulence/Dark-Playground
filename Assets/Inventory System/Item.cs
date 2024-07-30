using UnityEngine;
using UnityEngine.UI;

public abstract class Item
{
    public string Name { get; set; }
    public Sprite Icon { get; set; }
    public bool IsStackable { get; set; }
    public int MaxStackSize { get; set; }

    // Constructor to set up item properties
    protected Item(string name, Sprite icon, bool isStackable, int maxStackSize)
    {
        Name = name;
        Icon = icon;
        IsStackable = isStackable;
        MaxStackSize = maxStackSize;
    }

    // Method to use the item
    public abstract void Use();
}
