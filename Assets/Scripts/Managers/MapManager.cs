using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class MapManager : MonoBehaviour
{
    [Header("Player Interaction Stats")]
    [SerializeField]
    WaterSpell _waterSpell;
    public bool cultivatingSelected; // used when radial wheel / interacting is selected

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
    private List<TileData> tileDatas; // list of the different data types created 

    [Header("Crop Selected")] // link from inventory to know which crop object to instantiate
    public GameObject _cropSelected;
    [SerializeField]
    GameObject _cropListParent;

    [Header("References")]
    public CropSlotManagaer _cropSlotManager;


   


    private Dictionary<TileBase, TileData> dataFromTiles; // dictionary of the different tiles that have been made from the SO

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>(); // creates a dictionary of tile data

        foreach(var tileData in tileDatas)
        {
            foreach(var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData); //the tile is the key and the tile data is the value
            }
        }
    }

    private void Update()
    {
        _playerTransform = _player.transform;

        if (Input.GetMouseButtonDown(0)) // on left click, only gets it once not when held
        {
           
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //gets mouse position
            Vector3Int gridPosition = _interactableTileMap.WorldToCell(mousePosition); //get the grid cell position based on where the mouse input is

            TileBase clickedTile  = _interactableTileMap.GetTile(gridPosition); //gets which tile was clicked on at the position of the mouse cursor
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

                if (isTilledSoil == true && _cropSlotManager.CheckTileEmpty(gridPosition) == true)
                {
                    Debug.Log("Planting seed");
                    Instantiate(_cropSelected, gridPosition, transform.rotation, _cropListParent.transform); //create the crop selected at that grid position
                    //_cropSlotManager.cropSlots.Add(gridPosition , true);
                 
                    // if plant seeds action is selected
                    // can plant seeds here
                    

                }

                if(isTilledSoil == true && _cropSlotManager.CheckTileEmpty(gridPosition) == false) // a plant is currently here and can be watered
                {
                    Debug.Log("Watering");
                    WaterSoil();


                }
            }
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

    public void WaterSoil()
    {
        // access water bar UI on player
        // lower count so that it is visually represented
        // set tile to watered tile 
        // this tile should have data that makes growth speed go at a 1.25x modifier
        // crop needs to be able to access this modifier
        // this should be reset when new day begins so player should have to water plant again
    }

    //checks to see that where the player has clicked at and that it is within a certain range from them, if it isnt then they cant interact with it.
    public bool WithinRange()
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //gets mouse position
        float distance = Vector2.Distance(_playerTransform.position, mousePosition);

        if (distance < 2f) // if the player is within a 2 tile range then allow the input
        {
            return true;
        }
        else
        {         
           
            Debug.Log("Youre not close enough to interact with this tile");
            
            return false;
        }
    }
}
