using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    private bool menuActive = false;

    public ItemSlot[] itemSlots;

    // Item Description Slot
    public Image itemDescIcon;
    public TMP_Text itemDescNameText;
    public TMP_Text itemDescText;

    public Sprite[] testIcon;

    public int maxItemsNum = 64;

    private void Start()
    {
        DeselectAllSlots();
        UpdateDescriptionData(null, null, null);
        UpdateStates();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            int random = Random.Range(0, 10);
            int quantRand = Random.Range(1, 72);

            if (random == 0)
                AddItem("Big thing", quantRand, testIcon[random], "This description is so cool, wow. totally amazing. this is so sick. amazing woah wow cool");
            else if (random == 1)
                AddItem("Little thing", quantRand, testIcon[random], "This description is so cool, wow. totally amazing. this is so sick. amazing woah wow cool");
            else if (random == 2)
                AddItem("Boom", quantRand, testIcon[random], "This description is so cool, wow. totally amazing. this is so sick. amazing woah wow cool");
            else if (random == 3)
                AddItem("WOooah", quantRand, testIcon[random], "This description is so cool, wow. totally amazing. this is so sick. amazing woah wow cool");
            else if (random == 4)
                AddItem("Crazy", quantRand, testIcon[random], "This description is so cool, wow. totally amazing. this is so sick. amazing woah wow cool");
            else if (random == 5)
                AddItem("Im running out of random names", quantRand, testIcon[random], "This description is so cool, wow. totally amazing. this is so sick. amazing woah wow cool");
            else if (random == 6)
                AddItem("BONKERS", quantRand, testIcon[random], "This description is so cool, wow. totally amazing. this is so sick. amazing woah wow cool");
            else if (random == 7)
                AddItem("Wowzers", quantRand, testIcon[random], "This description is so cool, wow. totally amazing. this is so sick. amazing woah wow cool");
            else if (random == 8)
                AddItem("Amazing", quantRand, testIcon[random], "This description is so cool, wow. totally amazing. this is so sick. amazing woah wow cool");
            else if (random == 9)
                AddItem("Crikey", quantRand, testIcon[random], "This description is so cool, wow. totally amazing. this is so sick. amazing woah wow cool");
            else if (random == 10)
                AddItem("Moight", quantRand, testIcon[random], "This description is so cool, wow. totally amazing. this is so sick. amazing woah wow cool");
        }
    }

    void UpdateStates()
    {
        // When switching between active and not we can update the states
        // of the objects that would be affected by that change.
        inventoryMenu.SetActive(menuActive);
    }

    // Perform the open/close inventory logic on input.
    public void HandleInput(InputAction.CallbackContext cbContext)
    {
        // If not open but attempting to 'escape' out then just skip.
        if (cbContext.control.name == "escape" && !menuActive)
            return;

        menuActive = !menuActive;

        UpdateStates();
    }

    public int AddItem(string itemName, int quantity, Sprite itemIcon, string itemDescription)
    {
        for (int i = 0; i < itemSlots.Length; i++) 
        {
            if (!itemSlots[i].isFull && itemSlots[i].itemName == itemName || itemSlots[i].quantity == 0)
            {
                int leftOverItems = itemSlots[i].AddItem(itemName, quantity, itemIcon, itemDescription);
                if (leftOverItems > 0)
                    leftOverItems = AddItem(itemName, leftOverItems, itemIcon, itemDescription);

                return leftOverItems;
            }
        }

        return quantity;
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].selectedPanel.SetActive(false);
            itemSlots[i].isItemSelected = false;
        }
    }

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

    public void UpdateDescriptionData(string itemName, string itemDescription, Sprite itemIcon)
    {
        itemDescNameText.text = itemName;
        itemDescText.text = itemDescription;
        itemDescIcon.sprite = itemIcon;

        if (itemDescIcon.sprite == null)
            itemDescIcon.enabled = false;
        else
            itemDescIcon.enabled = true;
    }
}
