using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static StateController;

public class EconomyScreen : MonoBehaviour
{
    [Header("Player Stats")]
    public int goldEarned;
    public int currentGold;
    public int enemiesKilled;
    public int cropsPlanted;

    [Header("References")]
    [SerializeField] GameObject _economyScreen;
    [SerializeField] TextMeshProUGUI _goldEarned;
    [SerializeField] TextMeshProUGUI _currentGold;
    [SerializeField] TextMeshProUGUI _enemiesKilled;
    [SerializeField] TextMeshProUGUI _cropsPlanted;
    [SerializeField] GamePhase _gamePhase;
    [SerializeField] StateController _stateController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateStatScreen()
    {
        _cropsPlanted.text = string.Format("Crops Planted : {000}", cropsPlanted); // formats the text on the screen
        _goldEarned.text = string.Format("Gold Earned : {000}", goldEarned);
        _enemiesKilled.text = string.Format("Enemies Killed : {000}", enemiesKilled);
        _currentGold.text = string.Format("Your Gold : {000}", currentGold);
    }


    public void StartNewDay()
    {
        _economyScreen.SetActive(false);
        _stateController.ChangeState(EGameState.Paused);
        _gamePhase.StartDay();

        goldEarned = 0; // reset the players stats for the day (dont need to touch current gold as that should be whatever it is)
        enemiesKilled = 0; 
        cropsPlanted = 0;
}
}
