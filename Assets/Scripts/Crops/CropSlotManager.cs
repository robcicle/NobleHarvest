using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropSlotManagaer : MonoBehaviour
{
    [SerializeField]
    private Tilemap _dirtTileMap;

    public Dictionary<Vector3Int, bool> cropSlots = new Dictionary<Vector3Int, bool>();



    public bool CheckTileEmpty(Vector3Int gridPosition)
    {
        if (!cropSlots.ContainsKey(gridPosition)) // if it doesnt contain the key (being empty)
        {
            Debug.Log("Is currently empty");
            cropSlots.Add(gridPosition, true);  // add it to the dictionary
            return true;
        }

        else
        {
            Debug.Log("Is now occupied");
            //cropSlots.Remove(gridPosition); // dont need to remove here, do it when a crop is harvested
            return false; // if it is occupied, ignore

        }

    }

    
}
