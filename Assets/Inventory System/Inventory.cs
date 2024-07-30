using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    Dictionary<Item, int> items;

    int lastItem;

    public void AddItem(Item item)
    {
        items.Add(item, lastItem);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public void MoveItem(Item item, int newItemSlot)
    {
        items.Remove(item);
        items.Add(item, newItemSlot);
    }

    public Dictionary<Item, int> GetItems()
    {
        return items;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
