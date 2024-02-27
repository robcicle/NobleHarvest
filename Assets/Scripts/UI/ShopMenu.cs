using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [Header("Variables")]
    public int currentGold = 20; // gold the player currently has 
    int beetRootCost = 10;
    int mushroomCost = 50; // integer cost of the items 
    int lettuceCost = 150;
    int itemCost; // this is set to the cost of the thing being bought from the options above
    bool menuActive;

    [Header("References")]
    public GameObject _ShopMenu; // reference to the shop ui
    InventoryManager _inventoryManager;

    [Header("Shop UI Text References")]
    public TextMeshProUGUI _eldritchMushroomText; // the text cost of the item 
    public TextMeshProUGUI _icebergLettuceText;
    public TextMeshProUGUI _voidBeetText;
    public TextMeshProUGUI _currentGoldText; // text of current gold

    public GameObject itemSlots;

    public GameObject[] itemsToBuy; // rather than creating a buy script for each button, get index reference to the button, use that as a reference to the scriptable object

    

    // Start is called before the first frame update
    void Start()
    {
        itemsToBuy = new GameObject[itemSlots.transform.childCount];

        for(int i = 0; i <itemSlots.transform.childCount; i++) // populates the list with the children of the game object
        {
            itemsToBuy[i] = itemSlots.transform.GetChild(i).gameObject;
        }


        _inventoryManager = GetComponent<InventoryManager>(); // get a reference to the inventory manager to add items to

        _eldritchMushroomText.text = string.Format("Cost: {000}", mushroomCost); // formats the text to change the numbers within the curly bracket to be the variable selected
        _icebergLettuceText.text = string.Format("Cost: {000}", lettuceCost);
        _voidBeetText.text = string.Format("Cost: {000}", beetRootCost);




    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.I))
        {
            if(menuActive == false)
            {
                //Debug.Log("Opening shop");
                menuActive = true;
                _currentGoldText.text = string.Format("Gold: {0000}", currentGold);
                _ShopMenu.SetActive(true);
            }
            else
            {
               // Debug.Log("Closing Shop");
                _ShopMenu.SetActive(false);
                menuActive = false;
            }
        }
    }

    public void BuyItem()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;

        int index = Array.IndexOf(itemsToBuy, clickedButton); // get the index of the button in the array

        Debug.Log(index);
        // matches up the the index of the button with the index of the scriptable objects
        // this is just a manual way of doing so it would need changing if the scriptable objects array is changed
        // in the gamemanager
        switch (index)
        {
            case 0:
                itemCost = beetRootCost; 
                break;
            case 1:
                itemCost = mushroomCost;
                break;
            case 2:
                itemCost = lettuceCost;
                break;
               
        }


        if(currentGold >= itemCost)
        {        
            ItemSO item = ItemManager.instance.itemSOs[index]; // the item we are buying is the once at the index of the scriptable objects
            Debug.Log("You have Purchased " + item); // debuggin what we have bought
            _inventoryManager.ItemBought(item, 1 ,index); // buys 1 of the item referenced into the same index positon of the inventory grid
            currentGold-= itemCost; // subtract the cost from the players gold
            _currentGoldText.text = string.Format("Gold: {000}", currentGold); 
        }
        else
        {
            Debug.Log("You dont have enough gold."); // if the player doesnt have enough gold
            return;
        }
    }
}
