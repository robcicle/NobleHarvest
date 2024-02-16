using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    //player reference
    [SerializeField]
    GameObject _player;
    Transform _playerTransform;


    [SerializeField]
    private Tilemap map;

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
        _player = GameObject.FindWithTag("Player");

       


    }
    private void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
        }

        _playerTransform = _player.transform;

        


        if (Input.GetMouseButtonDown(0)) // on left click, only gets it once not when held
        {
           
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //gets mouse position
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            TileBase clickedTile = map.GetTile(gridPosition); //gets which tile was clicked on at the position of the mouse cursor

            bool isWaterTile = dataFromTiles[clickedTile].isWater;
            if (isWaterTile == true)
            {
                CollectWater();
                 Debug.Log("You have clicked on a " + clickedTile + ",this tile is water");
            }
        
            else
            {
                Debug.Log("This tile is not water");
            }
        }
    }

    public void CollectWater()
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //gets mouse position
        float distance = Vector2.Distance(_playerTransform.position, mousePosition);

        if (distance < 2f)
        {
            Debug.Log("Water Spell filled up");
            // fill the players water count
            // update water meter UI
            // play animation
        }
        else
        {
            Debug.Log("Youre not close enough to interact with this tile");
        }
    }
}
