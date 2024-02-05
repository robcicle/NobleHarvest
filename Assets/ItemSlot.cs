using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // Item Data
    public string itemName;
    public string itemDescription;
    public int quantity;
    public Sprite itemIcon;
    public bool isFull;

    // Slot Data
    [SerializeField]
    private TMP_Text quantityText;
    [SerializeField]
    private Image itemImage;

    public GameObject selectedPanel;
    public bool isItemSelected = false;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public int AddItem(string itemName, int quantity, Sprite itemIcon, string itemDescription)
    {
        // Check to see if slot is full
        if (isFull)
            return quantity;

        // Update Slot Data
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemIcon = itemIcon;

        itemImage.sprite = itemIcon;
        itemImage.enabled = true;

        // Update quantity
        this.quantity += quantity;
        if (this.quantity >= inventoryManager.maxItemsNum)
        {
            // Return leftovers
            int extraItems = this.quantity - inventoryManager.maxItemsNum;
            this.quantity = inventoryManager.maxItemsNum;

            quantityText.text = this.quantity.ToString();
            quantityText.enabled = true;
            isFull = true;

            return extraItems;
        }

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;
    }

    void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        selectedPanel.SetActive(true);
        isItemSelected = true;

        inventoryManager.UpdateDescriptionData(itemName, itemDescription, itemIcon);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }
}
