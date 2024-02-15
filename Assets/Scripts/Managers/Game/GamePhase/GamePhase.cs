using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.Universal;

public class GamePhase : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] int startEnemySpawning;
    [SerializeField] bool canEndDay;
    float timerInterval = 1;
    float currentTime; // the actual time
    int currentTimeIndex; // used to keep track of game states
    public bool haveEnemiesSpawned;

    // float lengthOfFullDay = 448; //keeping these two here for reference
    // float startOfNight = 280;
    //int hourInterval = 28; // number of seconds in an in-game hour


    //number of hours in day is 16
    //10 day hours and 6 night hours


    [Header("References")]
    public EnemySpawning _enemySpawning;
    public GameTimerUI _gameTimerUI;
    public Light2D _light2D;


    // Start is called before the first frame update
    void Start()
    {
        StartDay();
    
    }

    // Update is called once per frame
    void Update()
    {
       
        currentTime += (Time.deltaTime); // timer going up in seconds
        

        // currentTime % 7 == 0;
        if (currentTime >= timerInterval && canEndDay != true) //updates the game timer every 15 in-game minutes
        {
            //Debug.Log(currentTime);
            currentTime = 0;
            currentTimeIndex++;
            _gameTimerUI.UpdateTime();
        }

        // states of the game
        switch (currentTimeIndex)
        {
            // these case number are just the number of intervals lapped so in-game time would be this number * 7
            case 0:
                _light2D.color = Color.yellow;
                canEndDay = false;
                Debug.Log("Start Of Day");
                //set crop growth to 1x modifier
                break;

            case 40:      
                Debug.Log("Start Of Night");
                // set crop growth to 0x modifier
                NightTimeBegun(); //spawn enemies
                break;
            case 64:
                _light2D.color = Color.blue;
                canEndDay = true;
                Debug.Log("Can end day now");
                break;

            case 65:
                currentTimeIndex = 64;
                break;
                
        }


        // debugging
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartDay();
        }
        // debugging
        if (Input.GetKeyDown(KeyCode.P))
        {
            SkipToNight();
        }
    }

    public void NightTimeBegun()
    {
        if(haveEnemiesSpawned == false)
        {

            _enemySpawning.SpawnEnemies();
            haveEnemiesSpawned = true;

        }
    }

    //used either to skip time currently debugging
    public void StartDay()
    {
        Debug.Log("It's the morning");
        haveEnemiesSpawned = false;
        currentTimeIndex = 0;
        currentTime = 0;
        _gameTimerUI.NewDay();
    }

    public void SkipToNight()
    {
        Debug.Log("It's night time");
        currentTimeIndex = 40;
        _gameTimerUI.NightTime();
    }
}
