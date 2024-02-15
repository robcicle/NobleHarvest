using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimerUI : MonoBehaviour
{
    [Header("Variables")]
    int minutesUpdateInterval;
    int hoursUpdateInterval;
    int hourTime = 7;

    [Header("References")]
    [SerializeField] TextMeshProUGUI _hours;
    [SerializeField] TextMeshProUGUI _minutes;


    // Start is called before the first frame update
    void Start()
    {
        minutesUpdateInterval = 0;
        hoursUpdateInterval = 0;
        _hours.text = string.Format("{00}", hourTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTime()     
    {
        //Debug.Log("Spaghetti");
        minutesUpdateInterval++;
        hoursUpdateInterval++;

        if(hoursUpdateInterval % 4 == 0)
        {
            hourTime++;
        }

        // used to change the time between saying 15,30,45
        switch (minutesUpdateInterval)
        {
           case 0:
                _minutes.text = string.Format(":{00}", "00");
                break;
           case 1:
                _minutes.text = string.Format(":{00}", "15");
                break;
           case 2:
                _minutes.text = string.Format(":{00}", "30");
                break;
           case 3:
                _minutes.text = string.Format(":{00}", "45");
                break;
           case 4:
                _minutes.text = string.Format(":{00}", "00");
                minutesUpdateInterval = 0;
                break;
          
        }

        //displays the time of the hour
        // the case number is what the hour should be in game
        switch (hourTime)
        {
            case 7:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 8:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 9:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 10:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 11:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 12:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 13:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 14:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 15:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 16:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 17: //night time starts
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 18: 
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 19:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 20:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 21:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 22:
                _hours.text = string.Format("{00}", hourTime);
                break;
            case 23:
                _hours.text = string.Format("{00}", hourTime);
                break;
        }




    }


    //sets the time and the visuals to the respective hour and minutes
    public void NewDay()
    {
        hourTime = 7;
        _hours.text = string.Format("{00}", hourTime);
        minutesUpdateInterval = 0;
        hoursUpdateInterval = 0;
        _minutes.text = string.Format(":{00}", "00");
    }

    public void NightTime()
    {
        hourTime = 17;
        _hours.text = string.Format("{00}", hourTime);
        minutesUpdateInterval = 0;
        hoursUpdateInterval = 0;
        _minutes.text = string.Format(":{00}", "00");
    }
}
