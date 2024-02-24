using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    // Item Data
    public string itemName;          // Name of the item
    public string itemDescription;   // Description of the item
    public int defaultQuantity = 1;  // Default quantity of the item
    public Sprite itemIcon;          // Icon representing the item
    public GameObject _gameObject; // used for crops, refers to the crop to be planted

    // Item Category
    public InventoryManager.ItemCategories itemCategory = new();  // Category of the item

    // Use the item based on its category
    public void UseItem()
    {
        // Switch case to determine the action based on item category
        switch (itemCategory)
        {
            case InventoryManager.ItemCategories.Seeds:  // If the item category is Seeds
                CropManager.instance.PlaceSeed(); // Call the PlaceSeed function from CropManager
                InventoryManager.instance.RemoveItem(this); // Remove the used seed from inventory
                break;
            case InventoryManager.ItemCategories.Weapons:  // If the item category is Weapons
                // Currently no action for using weapons
                break;
            default:  // If the item category is not recognized
                Debug.LogError("ITEMSO.cs: " + itemName + " uses unrecognized category '" + itemCategory + "'");  // Log unrecognized category as an error
                break;
        }

        Debug.Log(itemName + " was used."); // Log that the item was used
    }
}