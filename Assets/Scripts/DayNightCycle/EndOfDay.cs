using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public class EndOfDay : MonoBehaviour
{

    [Header("Variables")]
    public bool canEndDay;
    public int moneyToEarn;
    bool stoodOnDoor;

    [Header("References")]
    public GamePhase _gamePhase;
    public GameObject _economyScreenUI;
    public StateController _stateController;
    [SerializeField] GameObject _canInteractText;
    [SerializeField] EconomyScreen _economyScreen;
    [SerializeField] ShopMenu _shopMenu;
    EnemySpawning _enemySpawning;

    public static EndOfDay instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        _canInteractText.SetActive(false);
        _shopMenu = GameObject.Find("Canvas").GetComponent<ShopMenu>();
        _economyScreen = GameObject.Find("Canvas").GetComponent<EconomyScreen>();
        _enemySpawning = GameObject.Find("Enemies").GetComponent<EnemySpawning>();
    }

        // Update is called once per frame
        void Update()
        {
            // allows the player to end the day and bring up the stat screen
            if (Input.GetKeyDown(KeyCode.R) && stoodOnDoor == true)
            {
                Time.timeScale = 0f; //pauses the game stopping player input
                _economyScreen.goldEarnedToday = moneyToEarn;
                _shopMenu.currentGold += moneyToEarn;
                _economyScreen.currentGold = _shopMenu.currentGold;
                _economyScreen.UpdateStatScreen(); // updates the stats shown on the end screen
                _economyScreenUI.SetActive(true); // puts the UI on screen
                _canInteractText.SetActive(false); // removes the interact text for when the UI is closed
                moneyToEarn = 0; // resets the gold earned for the day 
                _enemySpawning.enemiesToSpawn += 0.5f; // increases the number of enemies to spawn on subsequent days
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Debug.Log("rip bozo");
            if (collision.gameObject.tag == "Player" && canEndDay == true) //checks if the player is on the object and the time is correct
            {
                stoodOnDoor = true;
                _canInteractText.SetActive(true);

            }
        }

        // when the player leaves 
        private void OnTriggerExit2D(Collider2D collision)
        {
            stoodOnDoor = false;
            _canInteractText.SetActive(false);
        }
    
}
