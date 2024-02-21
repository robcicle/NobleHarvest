using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenGrowth = 15.0f;  // Time between each growth stage
    [SerializeField]
    private float maxStages = 4;  // Maximum number of growth stages

    private float growthTimer = 0.0f;  // Timer to track growth time
    bool isGrowing = false;            // Flag to indicate if the crop is currently growing
    int growthState = 1;               // Current growth stage of the crop

    // Start is called before the first frame update
    void Start()
    {
        isGrowing = true;  // Start the growth process
        GetComponent<SpriteRenderer>().sprite = CropManager.instance.cropSprites[growthState - 1];  // Set the initial sprite based on the growth state
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrowing) // If the crop is not growing, return
            return;

        growthTimer += Time.deltaTime;  // Increment the growth timer based on real time

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
        GetComponent<SpriteRenderer>().sprite = CropManager.instance.cropSprites[growthState - 1];  // Set the sprite for the new growth stage
    }
}