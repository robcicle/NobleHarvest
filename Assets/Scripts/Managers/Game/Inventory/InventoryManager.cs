using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    // Singleton instance
    public static InventoryManager instance = null;

    // References to UI objects
    [SerializeField]
    private GameObject inventoryMenu;  // Reference to the inventory menu
    private bool menuActive = false;  // Flag to track whether the inventory menu is open or not

    // Enum for item category types
    public enum ItemCategories
    {
        Seeds,
        Weapons
    }

    // Item Description Slot
    [SerializeField]
    private Image itemDescIcon;         // Reference to the image for the item's icon in the item description
    [SerializeField]
    private TMP_Text itemDescNameText;  // Reference to the text displaying the item name in the item description
    [SerializeField]
    private TMP_Text itemDescText;      // Reference to the text displaying the item description in the item description

    public int maxItemsNum = 64;  // Maximum number of items allowed in the inventory

    // Item Category data
    [SerializeField]
    private TMP_Text categoryText;
    private ItemCategories selectedCategory = 0;
    [SerializeField]
    private GameObject[] categorySlots;

    private void Awake()
    {
        // Assert if there is already a controller.
        Debug.Assert(instance == null,
            "Multiple instances of singleton has already been created",  // Assertion message for multiple instances
            this.gameObject  // Object associated with the assertion
            );

        // Handle of the first manager created.
        instance = this;  // Assign this instance as the singleton instance
    }

    private void Start()
    {
        // Initialize inventory
        DeselectAllSlots();  // Deselect all item slots
        UpdateDescriptionData(null, null, null);  // Update item description with null values
        UpdateCategoryData(0);
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
        ItemSlot[] itemSlots = categorySlots[(int)item.itemCategory].GetComponentsInChildren<ItemSlot>();

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
        return AddItem(item, item.defaultQuantity);
    }

    // Deselect all item slots in the inventory
    public void DeselectAllSlots()
    {
        ItemSlot[] itemSlots = gameObject.GetComponentsInChildren<ItemSlot>();

        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].DeselectSlot();
        }
    }

    // Check if the inventory is full
    public bool IsInvFull()
    {
        ItemSlot[] itemSlots = gameObject.GetComponentsInChildren<ItemSlot>();

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

    // Update the category data based on the increment amount
    public void UpdateCategoryData(int amtToInc)
    {
        int enumCount = System.Enum.GetNames(typeof(ItemCategories)).Length; // Get the number of enum values

        // Calculate the index of the next enum value
        int newIndex = ((int)selectedCategory + amtToInc + enumCount) % enumCount;

        // Update the current enum value
        selectedCategory = (ItemCategories)newIndex;

        categoryText.text = selectedCategory.ToString();

        for (int i = 0; i < System.Enum.GetNames(typeof(ItemCategories)).Length; i++)
        {
            if (i == newIndex)
                categorySlots[i].SetActive(true);
            else
                categorySlots[i].SetActive(false);
        }
    }
}