using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using UnityEngine;

public class EnemyVariableController : MonoBehaviour
{
    [Header("Variables")]
    public int cropsRemaining;

    [Header("Enemies")]
    public int numOfEnemies;
    private GameObject enemyListParent;
    private GameObject[] enemyList;

    [Header("references")]
    private EnemyBehaviour _enemyBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        enemyListParent = gameObject;


        //states an array of length number of enemies
        numOfEnemies = enemyListParent.transform.childCount;
        enemyList = new GameObject[numOfEnemies];

        // populates the array full of the gameobjects from the heirarchy
        for (int i = 0; i < numOfEnemies; i++)
        {
            enemyList[i] = enemyListParent.transform.GetChild(i).gameObject;
        }
    }

    /// <summary>
    // IMPORTANT 
    // WHEN A NEW PLANT IS ADDED IT MUST BE DECLARED AND ADDED TO THIS COUNTER
    // --> on start cropsRemaining++ <--
    //** on the crop prefab script
    /// </summary>

    public void CropKilled()
    {
       // cropsRemaining--;
    }

    public void UpdateEnemytargets()
    {
        cropsRemaining--;
        // for loop for how many children there are
        // change which script is being targeted based on the childs number
        // run the function to update the array in each child

        numOfEnemies = enemyListParent.transform.childCount;
        enemyList = new GameObject[numOfEnemies];

        // populates the array full of the gameobjects from the heirarchy
        for (int i = 0; i < numOfEnemies; i++)
        {
            //gets the script from each of the enemies in the array
            enemyList[i] = enemyListParent.transform.GetChild(i).gameObject;
            _enemyBehaviour = enemyList[i].GetComponent<EnemyBehaviour>();

            //removes the destroyed crop from the array
            _enemyBehaviour.ArrayCleanup();
        }



    }

    public void UpdateEnemyList()
    {

        for(int  i = 0; i < numOfEnemies; i++)
        {

        }
        // when an enemy is kiled update this list
    }
}
