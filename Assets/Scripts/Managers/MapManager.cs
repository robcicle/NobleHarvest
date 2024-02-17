using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [Header("Player Interaction Stats")]
    [SerializeField]
    WaterSpell _waterSpell;
    public bool cultivatingSeleceted; // used when radial wheel / interacting is selected

    //player reference
    [SerializeField]
    GameObject _player; 
    Transform _playerTransform;

    [Header("Tiles")]
    [SerializeField]
    private Tilemap _tileMap; // reference to the tilemap
    [SerializeField]
    private TileBase _tilledSoilTile; // reference of the tilled soil tile
    [SerializeField]
    private List<TileData> tileDatas; // list of the different data types created 


   


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
            Vector3Int gridPosition = _tileMap.WorldToCell(mousePosition);

            TileBase clickedTile = _tileMap.GetTile(gridPosition); //gets which tile was clicked on at the position of the mouse cursor

            bool isWaterTile = dataFromTiles[clickedTile].isWater;
            //bool isGrass = dataFromTiles[clickedTile].isGrass;
            bool isUntilledSoil = dataFromTiles[clickedTile].isNotTilledSoil;
            bool isTilledSoil = dataFromTiles[clickedTile].isTilledSoil;


            Debug.Log("You have clicked on  " + clickedTile);
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

                if (isTilledSoil == true)
                {
                    // if plant seeds action is selected
                    // can plant seeds here
                    // can water here if water is aquired
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
        _tileMap.SetTile(gridPosition, _tilledSoilTile);
        // change sprite to tilled soil
        // sprite should have scriptable object attacthed to it so it should have the data its supposed to have

        //game needs some way of knowing that soil is occupied by a crop
        //tilled soil that is not occupied by a crop needs to bre reset the following day

    }

    public void WaterSoil()
    {
        //get reference to crop being grown
        //increase grop growth modifier by 1.25x amount
        // this should be reset when new day begins so player should have to water plant again
    }

    //checks to see that where the player has clicked at and that it is within a certain range from them, if it isnt then they cant interact with it.
    public bool WithinRange()
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //gets mouse position
        float distance = Vector2.Distance(_playerTransform.position, mousePosition);

        if (distance < 2f)
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
