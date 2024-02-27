using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCostManager : MonoBehaviour
{

    // literally only controls this one variable
    public int removalCost = 500;

    // this script empty as hell
    public void IncreaseCost()
    {
        removalCost = removalCost * 2;
    }
}
