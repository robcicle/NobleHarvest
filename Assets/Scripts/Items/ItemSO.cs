using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int defaultQuantity = 1;
    public Sprite itemIcon;

    public InventoryManager.ItemCategories itemCategory = new InventoryManager.ItemCategories();

    public void UseItem()
    {
        switch (itemCategory) 
        {
            case InventoryManager.ItemCategories.Seeds:
                break;
            case InventoryManager.ItemCategories.Weapons:
                Debug.Log(itemName + " was used.");
                break;
            default:
                break;
        }
    }
}