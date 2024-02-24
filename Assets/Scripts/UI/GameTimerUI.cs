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
    int dayCounter;

    [Header("References")]
    [SerializeField] TextMeshProUGUI _hoursText;
    [SerializeField] TextMeshProUGUI _minutesText;
    [SerializeField] TextMeshProUGUI _dayCounterText;


    // Start is called before the first frame update
    void Start()
    {
        minutesUpdateInterval = 0;
        hoursUpdateInterval = 0;
        _hoursText.text = string.Format("{00}", hourTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTime()     
    {
        if(hourTime < 23)
        {
            minutesUpdateInterval++;
            hoursUpdateInterval++;
        }


        if(hoursUpdateInterval % 4 == 0)
        {
            hourTime++;
        }


        // used to change the time between saying 15,30,45
        switch (minutesUpdateInterval)
        {
           case 0:
                _minutesText.text = string.Format(":{00}", "00");
                break;
           case 1:
                _minutesText.text = string.Format(":{00}", "15");
                break;
           case 2:
                _minutesText.text = string.Format(":{00}", "30");
                break;
           case 3:
                _minutesText.text = string.Format(":{00}", "45");
                break;
           case 4:
                _minutesText.text = string.Format(":{00}", "00");
                minutesUpdateInterval = 0;
                break;

           
          
        }

        //displays the time of the hour
        // the case number is what the hour should be in game
        switch (hourTime)
        {
            case 7:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 8:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 9:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 10:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 11:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 12:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 13:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 14:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 15:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 16:
                _hoursText.text = string.Format("{00}", hourTime);
                break;

            case 17: //night time starts
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 18: 
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 19:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 20:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 21:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 22:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
            case 23:
                _hoursText.text = string.Format("{00}", hourTime);
                break;
        }




    }


    //sets the time and the visuals to the respective hour and minutes
    public void NewDay()
    {
        dayCounter++;
        _dayCounterText.text = string.Format("Day:{000}", dayCounter);
        hourTime = 7;
        _hoursText.text = string.Format("{00}", hourTime);
        minutesUpdateInterval = 0;
        hoursUpdateInterval = 0;
        _minutesText.text = string.Format(":{00}", "00");
    }

    public void NightTime()
    {
        hourTime = 17;
        _hoursText.text = string.Format("{00}", hourTime);
        minutesUpdateInterval = 0;
        hoursUpdateInterval = 0;
        _minutesText.text = string.Format(":{00}", "00");
    }
}
