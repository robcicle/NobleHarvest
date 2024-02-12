using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] int enemiesToSpawn = 5;
    public int difficulty = 1;

    [Header("Spawning")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    public void Update()
    { 
        //
        //debug 
        // 

        if (Input.GetKeyDown(KeyCode.T))
        {
            NightTimeBegun();
        }
    }

    public void NightTimeBegun()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        for(int i = 0; i < Mathf.RoundToInt(difficulty * enemiesToSpawn); i++)
        {
            //instantiates the enemys at a random interval, location and adds them as children to the parent of what the script is attached to
            //int randomTimer = Random.Range(2, 4);


            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            yield return new WaitForSeconds(0.1f);
            Instantiate(enemyPrefabs[0], spawnPoints[randomSpawnPoint].position, transform.rotation, this.transform);

            //To stop enemies spawning at parts of the map not unlocked use a case statement -
            //to make sure that it only spawns between certain numbers of the array.
            // or a bunch of if statements I don't mind.
        }
    }
}
