using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    public static CropManager instance = null; // Singleton instance

    public Sprite[] cropSprites; // Array to hold crop sprites

    public GameObject cropPrefab; // Prefab for the crop object

    private void Awake()
    {
        // Assert if there is already a controller.
        Debug.Assert(instance == null,
            "Multiple instances of singleton has already been created", // Assertion message for multiple instances
            this.gameObject // Object associated with the assertion
            );

        // Handle of the first controller created.
        instance = this; // Assign this instance as the singleton instance
    }

    // Function to place a seed
    public void PlaceSeed()
    {
        GameObject soilGO = GameObject.FindGameObjectWithTag("Soil"); // Find the soil game object using tag

        GameObject placedCrop = Instantiate(cropPrefab); // Instantiate a crop prefab

        placedCrop.transform.position = soilGO.transform.position; // Set the position of the crop object to the position of the soil
    }
}