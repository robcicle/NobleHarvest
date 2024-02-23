using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName; // Name of the item

    [SerializeField]
    private int quantity; // Quantity of the item

    [SerializeField]
    private Sprite itemIcon; // Icon representing the item

    [SerializeField]
    private string itemDescription; // Description of the item

    private InventoryManager inventoryManager; // Reference to the InventoryManager

    // Start is called before the first frame update
    void Start()
    {
        // Find and get the InventoryManager component from the "InventoryCanvas" GameObject
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
}