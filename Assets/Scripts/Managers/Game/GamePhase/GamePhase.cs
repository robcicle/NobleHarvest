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
    float timerInterval = 1.85f; // default was 7, felt a bit long
    float currentTime; // the actual time
    public int currentTimeIndex; // used to keep track of game states
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
    public Light2D _globalLight;
    public EndOfDay _endOfDay;
    public Light2D _playerLight;
    MapManager _mapManager;


    // Start is called before the first frame update
    void Start()
    {
        _mapManager = GameObject.Find("GameManager").GetComponent<MapManager>();
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
                _mapManager.isDayTime = true;
                _playerLight.intensity = 0f; // turn off the light on the player
                _light2D.color = Color.white; // changes the world lighting 
                _globalLight.intensity = 0.6f;
                _light2D.pointLightInnerRadius = 9; 
                canEndDay = false;
                //Debug.Log("Start Of Day");
                //set crop growth to 1x modifier
                break;

            case 30:
                _globalLight.intensity = 0.30f;
                break;
            case 40:
                _mapManager.isDayTime = false;
                //Debug.Log("Start Of Night");
                // set crop growth to 0x modifier
                _globalLight.intensity = 0.08f;
                NightTimeBegun(); //spawn enemies
                break;
            case 64:

                _endOfDay.canEndDay = true;
                //Debug.Log("Can end day now");
                break;

            case 65:
                currentTimeIndex = 64;
                break;
                
        }


        // debugging
        //if (Input.GetKeyDown(KeyCode.K))
        //{
         //   StartDay();
        //}
        // debugging
        if (Input.GetKeyDown(KeyCode.P))
        {
            SkipToNight();
        }
    }

    public void NightTimeBegun() // spawn enemies
    {
        _playerLight.intensity = 0.5f; // turn on the light on the player
        _light2D.color = Color.blue;
        _light2D.pointLightInnerRadius = 1; // changes the lighting to be dimmer and darker

        if (haveEnemiesSpawned == false)
        {

            _enemySpawning.SpawnEnemies();
            haveEnemiesSpawned = true;

        }
    }

    //used either to skip time
    public void StartDay()
    {
        //Debug.Log("It's the morning");
        haveEnemiesSpawned = false;
        currentTimeIndex = 0;
        currentTime = 0;
        _gameTimerUI.NewDay();
    }


    // this is mostly a debugging function
    public void SkipToNight()
    {
        //Debug.Log("It's night time");
        currentTimeIndex = 40;
        _gameTimerUI.NightTime();
    }
}
