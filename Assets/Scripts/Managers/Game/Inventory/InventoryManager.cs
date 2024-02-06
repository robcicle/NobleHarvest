using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    private bool menuActive = false;
    public ItemSlot[] itemSlots;

    public enum ItemCategories
    {
        None,
        Seeds,
        Weapons
    }

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
            int random = Random.Range(0, ItemManager.instance.itemSOs.Length);

            AddItem(ItemManager.instance.itemSOs[random]);

            if (random > 2)
                Debug.LogError("EWHOOOPS");
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

    public void UseItem(string itemName)
    {
        for (int i = 0; i < ItemManager.instance.itemSOs.Length; i++)
        {
            if (ItemManager.instance.itemSOs[i].itemName == itemName)
            {
                ItemManager.instance.itemSOs[i].UseItem();
                return;
            }
        }
    }

    public int AddItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < itemSlots.Length; i++) 
        {
            if (!itemSlots[i].isFull && itemSlots[i].itemName == item.itemName || itemSlots[i].quantity == 0)
            {
                int leftOverItems = itemSlots[i].AddItem(item, quantity);
                if (leftOverItems > 0)
                    leftOverItems = AddItem(item, leftOverItems);

                return leftOverItems;
            }
        }

        return quantity;
    }
    public int AddItem(ItemSO item)
    {
        for (int i = 0; i < itemSlots.Length; i++) 
        {
            if (!itemSlots[i].isFull && itemSlots[i].itemName == item.itemName || itemSlots[i].quantity == 0)
            {
                int leftOverItems = itemSlots[i].AddItem(item, item.defaultQuantity);
                if (leftOverItems > 0)
                    leftOverItems = AddItem(item, leftOverItems);

                return leftOverItems;
            }
        }

        return item.defaultQuantity;
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
