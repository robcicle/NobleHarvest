using JetBrains.Annotations;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        Seeds, // Category for seeds
        Weapons // Category for weapons
    }

    [SerializeField]
    private ItemSlot[] seedSlots; // Array to hold seed item slots
    [SerializeField]
    private ItemSlot[] weaponSlots; // Array to hold weapon item slots

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
    private ItemCategories selectedCategory = 0; // Default selected category
    [SerializeField]
    private GameObject[] categorySlots; // Array to hold category slots
    

    // lets the map manager know which crop is selected
    MapManager mapManager;

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
        mapManager = GameObject.Find("GameManager").GetComponent<MapManager>();
        
        // Initialize inventory

        // Setup the itemSlots for each category
        seedSlots = categorySlots[(int)ItemCategories.Seeds].GetComponentsInChildren<ItemSlot>(); // Assign seed slots from the corresponding category slot
        weaponSlots = categorySlots[(int)ItemCategories.Weapons].GetComponentsInChildren<ItemSlot>(); // Assign weapon slots from the corresponding category slot

        DeselectAllSlots();  // Deselect all item slots
        UpdateDescriptionData(null, null, null);  // Update item description with null values
        UpdateCategoryData(0); // Update the category data with the default selected category
        UpdateStates();      // Update the states of UI objects

        for(int i = 0; i < ItemManager.instance.itemSOs.Length; i++)
        {

            var item = ItemManager.instance.itemSOs[i];
          
            StartCoroutine(SortInventory(item,i));

            
        }


         var firstItem = ItemManager.instance.itemSOs[0];
         AddItem(firstItem, 3); // add 3 of the 1st item in the inventory
        
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab)) // using only pc input
        {
            inventoryMenu.SetActive(!menuActive);
            menuActive = !menuActive;
        }
        // DEBUG FOR GIVING PLAYER A RANDOM ITEM
        //if (Input.GetKeyDown(KeyCode.J))
        //{            
        //    for (int i = 0; i < 10; i++)
        //    {
        //        int random = Random.Range(0, ItemManager.instance.itemSOs.Length);  // Get a random index within the range of itemSOs array
        //        AddItem(ItemManager.instance.itemSOs[random]);  // Add the randomly selected item to the inventory
        //    }

           
        //}
        //else if (Input.GetKeyDown(KeyCode.K))
        //{
        //    int random = Random.Range(0, ItemManager.instance.itemSOs.Length);  // Get a random index within the range of itemSOs array

        //    RemoveItem(ItemManager.instance.itemSOs[random]);  // Add the randomly selected item to the inventory
        //}
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
                //ItemManager.instance.itemSOs[i].UseItem();  // select the item

                var item = ItemManager.instance.itemSOs[i];
                ItemSlot[] itemSlots = GetSlotsFromCategory(item.itemCategory); // Get slots from the corresponding category

                
                mapManager.itemSelectedIndex = i; // reference to what scriptable object is selected
                //Debug.Log("Index number " + i);
                Debug.Log("You have selected " + ItemManager.instance.itemSOs[i] + " You have " + itemSlots[i].quantity); // logs the one selected
                mapManager._cropSelected = ItemManager.instance.itemSOs[i]._gameObject; // sets the crop to be planted as the one in inventory
                mapManager.itemIsSelected = true; // stops an issue that occurs when the player tries planting something at the very beggining with nothing selected
                return;
            }
        }
    }

    // Add an item to the inventory with a specified quantity.
    public int AddItem(ItemSO item, int quantity )
    {
        ItemSlot[] itemSlots = GetSlotsFromCategory(item.itemCategory); // Get slots from the corresponding category

        // Loop through the slots array.
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isFull && itemSlots[i].itemName == item.itemName || itemSlots[i].quantity == 0)
            {

                int leftOverItems = itemSlots[i].AddItem(ItemManager.instance.itemSOs[i], quantity);  // Add the item to the item slot
                if (leftOverItems > 0)
                    leftOverItems = AddItem(ItemManager.instance.itemSOs[i], leftOverItems);  // Add the remaining items recursively

                return leftOverItems;
            }
        }

        return quantity;
    }

    // Add an item to the inventory with its default quantity
    public int AddItem(ItemSO item)
    {
        return AddItem(item, item.defaultQuantity); // Add item with default quantity
    }

    public void ItemBought(ItemSO item, int quantity, int index) // adds the item at a specific slot in the inventory
    {
        ItemSlot[] itemSlots = GetSlotsFromCategory(item.itemCategory); // Get slots from the corresponding category
        int leftOverItems = itemSlots[index].AddItem(ItemManager.instance.itemSOs[index], quantity);  // Add the item to the item slot
        if (leftOverItems > 0)
            leftOverItems = AddItem(ItemManager.instance.itemSOs[index], leftOverItems);  // Add the remaining items recursively
   
    }


    // Remove an item from the inventory.
    public void RemoveItem(ItemSO item, int amountToRemove = 1)
    {
        ItemSlot[] itemSlots = GetSlotsFromCategory(item.itemCategory); // Get slots from the corresponding category

        // Loop through the slots array.
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].itemName == item.itemName && itemSlots[i].quantity > 0)
            {
                itemSlots[i].RemoveItem(amountToRemove);  // Remove one of the item from the item slot.

                return;
            }
        }
    }

    // Deselect all item slots in the inventory
    public void DeselectAllSlots()
    {
        ItemSlot[] itemSlots = gameObject.GetComponentsInChildren<ItemSlot>(); // Get all item slots

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

    // Get item slots corresponding to the specified category
    ItemSlot[] GetSlotsFromCategory(ItemCategories category)
    {
        ItemSlot[] itemSlots;

        // Switch case to determine the category
        switch (category)
        {
            case InventoryManager.ItemCategories.Seeds:  // If the item category is Seeds
                itemSlots = seedSlots;  // Assign seed slots
                break;
            case InventoryManager.ItemCategories.Weapons:  // If the item category is Weapons
                itemSlots = weaponSlots;  // Assign weapon slots
                break;
            default:  // If the item category is not recognized
                itemSlots = null;  // Assign null if category is not recognized
                Debug.LogError("INVENTORYMANAGER.cs: request to get unrecognized category '" + category + "'");  // Log unrecognized category as an error
                break;
        }

        return itemSlots;  // Return the item slots corresponding to the specified category
    }


    // called from mapmanager script to check if there are seeds available
    public bool IsItemSlotEmpty(ItemSO item, int itemSelected, string itemName)
    {
        ItemSlot[] itemSlots = GetSlotsFromCategory(item.itemCategory); // Get slots from the corresponding category

        if ((itemSlots[itemSelected] == null || itemSlots[itemSelected].quantity <= 0))
        {
            
            //Debug.Log("there are no items");
            return true; // if there was no item selected in the first place then the slot is also empty
        }
        else
        {
            //ItemManager.instance.itemSOs[itemSelected].UseItem(); // uses the item in the inventory when planted
            itemSlots[itemSelected].RemoveItem(1);  // Remove one of the item from the item slot.
            Debug.Log(itemSlots[itemSelected].quantity + " Seeds Remaining");
            //Debug.Log("there are items");
            return false; // if there are no items, stop referencing the seed to plant
        }
                                                         
    }

    IEnumerator SortInventory(ItemSO item, int i)
    {
        //ItemSlot[] itemSlots = GetSlotsFromCategory(item.itemCategory); // Get slots from the corresponding category
        
        AddItem(item);  // adds the items in order of how they appear onto the inventory, this avoids issues with them being in different slots
        yield return new WaitForEndOfFrame(); // gives time for the first function to run
        //itemSlots[i].quantity -= 1; // this adds all the items in inventory with 0 values, just keeps their sprites and descriptions there
        //itemSlots[i].quantityText.text = 0.ToString();

        RemoveItem(item);
    }



}