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
    public bool cultivatingSeleceted;

    //player reference
    [SerializeField]
    GameObject _player;
    Transform _playerTransform;

    [Header("Tiles")]
    [SerializeField]
    private Tilemap _tileMap;
    [SerializeField]
    private TileBase _tilledSoilTile;
    [SerializeField]
    private List<TileData> tileDatas;


   


    private Dictionary<TileBase, TileData> dataFromTiles; // dictionary of the different tiles that have been made from the SO

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>(); // creates the dictionary

        foreach(var tileData in tileDatas)
        {
            foreach(var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData); //the tile is the key and the tile data is the value
            }
        }
    }

    private void Start()
    {
        _player = GameObject.FindWithTag("Player"); // remove all this when the manager gets added to the game scene
        _waterSpell = _player.GetComponentInChildren<WaterSpell>();
       


    }
    private void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player"); // temporary fix while this script is not added to the game scene
            _waterSpell = _player.GetComponentInChildren<WaterSpell>();
        }

        _playerTransform = _player.transform;

        


        if (Input.GetMouseButtonDown(0)) // on left click, only gets it once not when held
        {
           
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //gets mouse position
            Vector3Int gridPosition = _tileMap.WorldToCell(mousePosition);

            TileBase clickedTile = _tileMap.GetTile(gridPosition); //gets which tile was clicked on at the position of the mouse cursor

            bool isWaterTile = dataFromTiles[clickedTile].isWater;
            bool isGrass = dataFromTiles[clickedTile].isGrass;
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
    }

    public void WaterSoil()
    {
        //get reference to crop being grown
        //increase grop growth modifier by 1.25x amount
        // this should be reset when new day begins so player should have to water plant again
    }

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
