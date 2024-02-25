using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Crop : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenGrowth = 15.0f;  // Time between each growth stage
    [SerializeField]
    private float maxStages = 4;  // Maximum number of growth stages

    private float growthTimer = 0.0f;  // Timer to track growth time
    bool isGrowing = false;            // Flag to indicate if the crop is currently growing
    int growthState = 1;               // Current growth stage of the crop



    [SerializeField]
    private GamePhase _gamePhase; // to check if its day or night

    public float growthModifier  = 1f; // used to switch between day, night and watered growth.
                                       //1 for day, 0 for night and 1.25x 

    // deals with the crops position in relation with the grid
    public CropSlotManager _cropSlotManager; //used to check if the soil is watered or not 
    Vector3Int currentPosition;
    [SerializeField]
    Tilemap _interactableTileMap;

    //list of sprites to change to
    [SerializeField]
    List<Sprite> cropSprites;
    SpriteRenderer _spriteRenderer;





    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager"); // used to get references to the managers
        _gamePhase = gameManager.GetComponent<GamePhase>();
        _cropSlotManager = gameManager.GetComponent<CropSlotManager>();
        _interactableTileMap = GameObject.Find("InteratableTileMap(Dirt Layer)").GetComponent<Tilemap>();
        currentPosition = _interactableTileMap.WorldToCell(gameObject.transform.position); //gets the grid location of the gameobject

        isGrowing = true;  // Start the growth process
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = cropSprites[0];
        //GetComponent<SpriteRenderer>().sprite = CropManager.instance.cropSprites[growthState - 1];  // Set the initial sprite based on the growth state
    }

    // Update is called once per frame
    void Update()
    {
        switch (_gamePhase.currentTimeIndex) //checks the current time 
        {
            case 0:
                growthModifier = 1f; // if its day time, grow normally
                break;
            case 40:
                growthModifier = 0f; // if its night time, dont grow
                break;
        }

        // seperate checks need to be done to see if the tile is watered, but it being night time should take priority
        if (_cropSlotManager.CheckTileWatered(currentPosition) == true && _gamePhase.currentTimeIndex < 40) // if the tile where the crop is planted, is watered, then the growth modifier is increased
        {
            growthModifier = 1.25f;
        }




                if (!isGrowing) // If the crop is not growing, return
            return;

        growthTimer += Time.deltaTime * growthModifier;  // Increment the growth timer based on real time

        if (growthTimer >= timeBetweenGrowth)  // If the growth timer reaches the time between growth stages
        {
            growthTimer = 0.0f;  // Reset the growth timer
            Grow();              // Grow the crop to the next stage
        }
    }

    // Method to grow the crop to the next stage
    void Grow()
    {
        if (growthState + 1 > maxStages)  // If the next growth stage exceeds the maximum stages
        {
            isGrowing = false;  // Stop the growth process
            return;
        }


        growthState++; // Increment the growth stage


        //int cropSpriteNum = 0;
        //cropSpriteNum++;
        _spriteRenderer.sprite = cropSprites[growthState - 1]; // goes through the list of sprites for the crop game object
        //GetComponent<SpriteRenderer>().sprite = CropManager.instance.cropSprites[growthState - 1];  // Set the sprite for the new growth stage
    }
}