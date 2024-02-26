using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [Header("Player Interaction Stats")]
    [SerializeField]
    WaterSpell _waterSpell;
    public bool cultivatingSelected = false; // used when radial wheel / interacting is selected
    public int waterSoilInterval = 60; // how often the player must water the crops in seconds

    //player reference
    [SerializeField]
    GameObject _player;
    Transform _playerTransform;

    [Header("Tiles")]
    [SerializeField]
    private Tilemap _interactableTileMap; // reference to the tilemap
    [SerializeField]
    private TileBase _tilledSoilTile; // reference of the tilled soil tile
    [SerializeField]
    private TileBase _wateredSoilTile;
    [SerializeField]
    private List<TileData> tileDatas; // list of the different data types created 

    [Header("Crop Selected")] // link from inventory to know which crop object to instantiate
    public GameObject _cropSelected;
    [SerializeField]
    GameObject _cropListParent;
    CropManager _cropManager;

    [Header("References")]
    public CropSlotManager _cropSlotManager;
    public EconomyScreen _economyScreen;







    private Dictionary<TileBase, TileData> dataFromTiles; // dictionary of the different tiles that have been made from the SO

    private void Awake()
    {
        _cropManager = GetComponent<CropManager>();

        dataFromTiles = new Dictionary<TileBase, TileData>(); // creates a dictionary of tile data

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData); //the tile is the key and the tile data is the value
            }
        }
    }

    private void Update()
    {
        _playerTransform = _player.transform;

        if (Input.GetMouseButtonDown(0) && cultivatingSelected == true) // on left click, only gets it once not when held
        {

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //gets mouse position
            Vector3Int gridPosition = _interactableTileMap.WorldToCell(mousePosition); //get the grid cell position based on where the mouse input is

            TileBase clickedTile = _interactableTileMap.GetTile(gridPosition); //gets which tile was clicked on at the position of the mouse cursor
            //Debug.Log("You have clicked on  " + clickedTile);



            // check the tile you click on against the ones in the dictionary to see if the value matches

            bool isWaterTile = dataFromTiles[clickedTile].isWater;
            //bool isGrass = dataFromTiles[clickedTile].isGrass;
            bool isUntilledSoil = dataFromTiles[clickedTile].isNotTilledSoil;
            bool isTilledSoil = dataFromTiles[clickedTile].isTilledSoil;
            //bool isOccupied = dataFromTiles[clickedTile].tileIsOccupied;




            if (WithinRange())
            {

                if (isWaterTile == true)
                {
                    CollectWater();

                }
                //if (isGrass == true)
                // {
                // dont actually know why i made this, might do something with it 
                //}

                if (isUntilledSoil == true)
                {
                    TillSoil(gridPosition);
                    // if till soil option is selected
                    // cultivate soil and change it to tilled soil
                }

                if (isTilledSoil == true && _cropSlotManager.CheckTileEmpty(gridPosition) == true) // if the slot is empty, try plant a seed
                {
                    if (_cropSlotManager.CheckTileWatered(gridPosition) == true) // can also be planted if the soil is watered
                    {
                        PlantCrop(gridPosition);
                    }
                    else
                    {
                        PlantCrop(gridPosition);
                    }
                    //Debug.Log("Planting seed");


                    //_cropSlotManager.cropSlots.Add(gridPosition , true);

                    // if plant seeds action is selected
                    // can plant seeds here


                }
                else if (_cropSlotManager.CheckTileEmpty(gridPosition) == false && _waterSpell.currentWaterLevel > 0) // if the grid slot is occupied water the soil instead if there is water available
                {

                    Debug.Log("Watering");
                    WaterSoil(gridPosition);


                    //    Debug.Log("Tile is watered already");



                }
            }
        }
    }

    public void PlantCrop(Vector3Int gridPosition)
    {
        //_cropSelected = ItemManager.instance.itemSO[0]; // this selects the crop to be used, referenced from the inventory
        // requires a second check to see if there are any remaining, if the number count is zero, do nothing.
        // if the number of items in  the inventory is greater than 0 then use that as reference to instantiate
        // if it is 0 then remove the reference to it so that is doesnt plant anything

        if (_cropSelected != null) // if there is a game object selected, plant it 
        {
            Debug.Log("Planted crop");
            _cropSlotManager.cropSlots.Add(gridPosition, true); // if a crop has been planted then add it to the dictionary of occupied slots
            Instantiate(_cropSelected, gridPosition, transform.rotation, _cropListParent.transform); //create the crop selected at that grid position
            _economyScreen.cropsPlanted++; // changes the end of day stats to represent what the player has done
        }
        else
        {
            Debug.Log("No crop selected");
            return;
        }
    }

    public void CollectWater()
    {

        _waterSpell.FillWaterMeter();

        //Debug.Log("Water Spell filled up");


        // update water meter UI
        // play animation      
    }

    public void TillSoil(Vector3Int gridPosition)
    {
        Debug.Log("Tilling the soil");
        _interactableTileMap.SetTile(gridPosition, _tilledSoilTile);
        // change sprite to tilled soil
        // sprite should have scriptable object attacthed to it so it should have the data its supposed to have

        //tilled soil that is not occupied by a crop needs to be reset the following day

    }

    public void WaterSoil(Vector3Int gridPosition)
    {
        if (_cropSlotManager.CheckTileWatered(gridPosition) == true) // if the tile is already watered, do nothing
        {
            return;
        }
        else
        {
            _interactableTileMap.SetTile(gridPosition, _wateredSoilTile); // changes the sprite
            _waterSpell.UseWater(); // changes the UI so its correct
            _cropSlotManager.wateredTiles.Add(gridPosition, true); // states the tile is watered in the dictionary

            StartCoroutine(ResetTile(gridPosition)); // sets tile back to normal after a period of time where it must be watered again
        }

    }

    IEnumerator ResetTile(Vector3Int gridPosition)
    {
        yield return new WaitForSeconds(waterSoilInterval);
        _interactableTileMap.SetTile(gridPosition, _tilledSoilTile);
        _cropSlotManager.wateredTiles.Remove(gridPosition);
    }

    //checks to see that where the player has clicked at and that it is within a certain range from them, if it isnt then they cant interact with it.
    public bool WithinRange()
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //gets mouse position
        float distance = Vector2.Distance(_playerTransform.position, mousePosition);

        if (distance < 2.5f) // if the player is within a 2 tile range then allow the input
        {
            return true;
        }
        else
        {


            //Debug.Log("Youre not close enough to interact with this tile");

            return false;
        }
    }

    public void CropRemoved(Vector2 cropPosition) // call this when a crop is destroyed or harvested
    {
        Vector3Int gridPosition = _interactableTileMap.WorldToCell(cropPosition);
        _cropSlotManager.cropSlots.Remove(gridPosition);
    }
}
