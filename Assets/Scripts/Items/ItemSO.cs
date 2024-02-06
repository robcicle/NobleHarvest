using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    // Item Data
    public string itemName;          // Name of the item
    public string itemDescription;   // Description of the item
    public int defaultQuantity = 1;  // Default quantity of the item
    public Sprite itemIcon;          // Icon representing the item

    // Item Category
    public InventoryManager.ItemCategories itemCategory = new InventoryManager.ItemCategories();  // Category of the item

    // Use the item based on its category
    public void UseItem()
    {
        // Switch case to determine the action based on item category
        switch (itemCategory)
        {
            case InventoryManager.ItemCategories.Seeds:  // If the item category is Seeds
                break;
            case InventoryManager.ItemCategories.Weapons:  // If the item category is Weapons
                Debug.Log(itemName + " was used.");  // Log that the item was used
                break;
            default:  // If the item category is not recognized
                Debug.LogError(itemName + " uses unrecognized category '" + itemCategory + "'");  // Log unrecognized category as an error
                break;
        }
    }
}