using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // References to UI objects
    public GameObject inventoryMenu;  // Reference to the inventory menu
    private bool menuActive = false;  // Flag to track whether the inventory menu is open or not
    public ItemSlot[] itemSlots;      // Array of item slots for the inventory slots

    // Enum for item category types
    public enum ItemCategories
    {
        Seeds,
        Weapons
    }

    // Item Description Slot
    public Image itemDescIcon;         // Reference to the image for the item's icon in the item description
    public TMP_Text itemDescNameText;  // Reference to the text displaying the item name in the item description
    public TMP_Text itemDescText;      // Reference to the text displaying the item description in the item description

    public int maxItemsNum = 64;  // Maximum number of items allowed in the inventory

    private void Start()
    {
        // Initialize inventory
        DeselectAllSlots();  // Deselect all item slots
        UpdateDescriptionData(null, null, null);  // Update item description with null values
        UpdateStates();      // Update the states of UI objects
    }

    private void Update()
    {
        // DEBUG FOR GIVING PLAYER A RANDOM ITEM
        if (Input.GetKeyDown(KeyCode.J))
        {
            int random = Random.Range(0, ItemManager.instance.itemSOs.Length);  // Get a random index within the range of itemSOs array

            AddItem(ItemManager.instance.itemSOs[random]);  // Add the randomly selected item to the inventory
        }
    }

    // Update the state of the inventory menu aka open or close it.
    void UpdateStates()
    {
        // When switching between active and not, update the states of the objects that would be affected by that change.
        inventoryMenu.SetActive(menuActive);
    }

    // Handle input to open/close inventory
    public void HandleInput(InputAction.CallbackContext cbContext)
    {
        // If the inventory is not open but attempting to 'escape' out, then skip.
        if (cbContext.control.name == "escape" && !menuActive)
            return;

        // Toggle the menuActive flag
        menuActive = !menuActive;

        // Update the states of UI objects
        UpdateStates();
    }

    // Use an item by its name.
    // Might be better to use the slot instead (assign each an index and make it pass that
    // through when using), especially if an item is to be deducted on use as this would
    // just grab the first one that it finds instead. Atp, not needed but if so in the
    // future then it's an easy switch.
    public void UseItem(string itemName)
    {
        // Loop through the scriptable items array.
        for (int i = 0; i < ItemManager.instance.itemSOs.Length; i++)
        {
            if (ItemManager.instance.itemSOs[i].itemName == itemName)
            {
                ItemManager.instance.itemSOs[i].UseItem();  // Use the item
                return;
            }
        }
    }

    // Add an item to the inventory with a specified quantity.
    public int AddItem(ItemSO item, int quantity)
    {
        // Loop through the slots array.
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isFull && itemSlots[i].itemName == item.itemName || itemSlots[i].quantity == 0)
            {
                int leftOverItems = itemSlots[i].AddItem(item, quantity);  // Add the item to the item slot
                if (leftOverItems > 0)
                    leftOverItems = AddItem(item, leftOverItems);  // Add the remaining items recursively

                return leftOverItems;
            }
        }

        return quantity;
    }

    // Add an item to the inventory with its default quantity
    public int AddItem(ItemSO item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isFull && itemSlots[i].itemName == item.itemName || itemSlots[i].quantity == 0)
            {
                int leftOverItems = itemSlots[i].AddItem(item, item.defaultQuantity);  // Add the item to the item slot with its default quantity
                if (leftOverItems > 0)
                    leftOverItems = AddItem(item, leftOverItems);  // Add the remaining items recursively

                return leftOverItems;
            }
        }

        return item.defaultQuantity;
    }

    // Deselect all item slots in the inventory
    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].selectedPanel.SetActive(false);  // Deactivate the selected panel
            itemSlots[i].isItemSelected = false;          // Set isItemSelected flag to false
        }
    }

    // Check if the inventory is full
    public bool IsInvFull()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isFull)
            {
                return false;
            }
        }

        return true;
    }

    // Update the data in the item description panel
    public void UpdateDescriptionData(string itemName, string itemDescription, Sprite itemIcon)
    {
        itemDescNameText.text = itemName;     // Update the item name text
        itemDescText.text = itemDescription;  // Update the item description text
        itemDescIcon.sprite = itemIcon;       // Update the item icon

        if (itemDescIcon.sprite == null)
            itemDescIcon.enabled = false;  // Disable the item icon if the sprite is null
        else
            itemDescIcon.enabled = true;   // Enable the item icon if the sprite is not null
    }
}