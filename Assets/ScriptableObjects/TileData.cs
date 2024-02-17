using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;

    public bool isWater, isTilledSoil, isNotTilledSoil, isGrass; // what kind of tile it is



}
