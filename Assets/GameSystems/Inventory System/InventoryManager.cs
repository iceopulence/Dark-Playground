using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;


    [SerializeField] private List<ItemSO> hotbarItems = new List<ItemSO>(5);
    public ItemSO heldItem;

    public ItemSO startingItem;

    private Dictionary <KeyCode, int> keyIndexMap;

    List<string> keysHeld = new List<string>();
    
    private void Awake()
    {

        keyIndexMap = new Dictionary<KeyCode,int>{
        {KeyCode.Alpha1, 0} , {KeyCode.Alpha2, 1}, {KeyCode.Alpha3, 2}, {KeyCode.Alpha4, 3}, {KeyCode.Alpha5,4}
    };
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AddItem(startingItem);

    }

    void Update()
    {
        foreach(var keyCodePair in keyIndexMap)
        {
            if(Input.GetKeyDown(keyCodePair.Key))
            {
                SelectItem(keyCodePair.Value);
            }
        }

        
    }

    public void AddItem(ItemSO newItem)
    {
        if (hotbarItems.Count < 5)
        {
            hotbarItems.Add(newItem);
            newItem.OnPickup();
        }
        else
        {
            Debug.Log("Hotbar full. Swap or drop an item.");
        }
    }

    public void UseItem(int slot)
    {
        if (slot >= 0 && slot < hotbarItems.Count)
        {
            hotbarItems[slot].Use();
        }
    }

    public void SelectItem(int itemIndex)
    {

        if(hotbarItems[itemIndex] == null)
        {
            return;
        }
        // tell anim controller to take out that item
        // save a reference to the item held

        heldItem = hotbarItems[itemIndex];
        GameManager.Instance.playerAnimController.TakeOutObject();
    }

    public void SwapItems(int slot1, int slot2)
    {
        ItemSO temp = hotbarItems[slot1];
        hotbarItems[slot1] = hotbarItems[slot2];
        hotbarItems[slot2] = temp;
        Debug.Log("Items swapped.");
    }

    public void DropItem(int slot)
    {
        if (slot < hotbarItems.Count)
        {
            hotbarItems[slot].OnDrop();
            hotbarItems.RemoveAt(slot);
        }
    }

     public void AddKey(string keyAdded)
    {
        keysHeld.Add(keyAdded);
    }
    public bool CheckHasKey(string keyChecked)
    {
       return keysHeld.Contains(keyChecked);
    }
}