using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // Item Data
    public string itemName = "";          // Name of the item
    public int quantity = 0;             // Quantity of the item
    public bool isFull = false;              // Flag indicating if the slot is full
    private string itemDescription = "";  // Description of the item
    private Sprite itemIcon = null;         // Icon representing the item

    // Slot Data
    [SerializeField]
    private TMP_Text quantityText;  // Text displaying the quantity of the item in the slot
    [SerializeField]
    private Image itemImage;        // Image representing the item

    [SerializeField]
    private GameObject selectedPanel;     // Panel indicating the selected item
    private bool isItemSelected = false;  // Flag indicating if the item is selected

    // Add an item to the slot with a specified quantity
    public int AddItem(ItemSO item, int quantity)
    {
        // Check if the slot is full
        if (isFull)
            return quantity;

        // Update slot data with item information
        this.itemName = item.itemName;
        this.itemDescription = item.itemDescription;
        this.itemIcon = item.itemIcon;

        itemImage.sprite = itemIcon;
        itemImage.enabled = true;

        // Update quantity
        this.quantity += quantity;
        if (this.quantity >= InventoryManager.instance.maxItemsNum)
        {
            // Handle excess items beyond maximum allowed
            int extraItems = this.quantity - InventoryManager.instance.maxItemsNum;
            this.quantity = InventoryManager.instance.maxItemsNum;

            quantityText.text = this.quantity.ToString();
            quantityText.enabled = true;
            isFull = true;

            return extraItems;
        }

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;
    }

    // Remove an item from a slot.
    public void RemoveItem(int amountToRemove)
    {
        // Take away one from the quantity.
        this.quantity-= amountToRemove;

        // Now update the itemSlot and other info to remove it.
        if (this.quantity <= 0)
        {
            this.quantity = 0;

            // Update slot data with item information
            this.itemName = "";
            this.itemDescription = "";
            this.itemIcon = null;

            itemImage.sprite = null;
            itemImage.enabled = false;

            quantityText.text = this.quantity.ToString();
            quantityText.enabled = false;
        }
        // Otherwise just update the quantity UI text.
        else
        {
            quantityText.text = this.quantity.ToString();
        }
    }

    // Deselect the item slot
    public void DeselectSlot()
    {
        selectedPanel.SetActive(false);  // Deactivate the selected panel
        isItemSelected = false;          // Set isItemSelected flag to false
    }

    // Handle left click action
    void OnLeftClick()
    {
        if (isItemSelected)
        {
            InventoryManager.instance.UseItem(itemName); // Use the item if it's already selected
            return;
        }

        // Deselect all other slots and select this one
        InventoryManager.instance.DeselectAllSlots();
        selectedPanel.SetActive(true);
        isItemSelected = true;

        // Update item description data
        InventoryManager.instance.UpdateDescriptionData(itemName, itemDescription, itemIcon);
    }

    // Handle pointer click event
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(); // Perform left click action
        }
    }
}