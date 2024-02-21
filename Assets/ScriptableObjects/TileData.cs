using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu]
public class TileData : ScriptableObject
{
    public TileBase[] tiles; // used to show what tiles are included in that data type

    public bool isWater, isTilledSoil, isNotTilledSoil, isGrass; // what kind of tile it is

 
}
