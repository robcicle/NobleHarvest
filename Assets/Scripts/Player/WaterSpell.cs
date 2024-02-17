using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterSpell : MonoBehaviour
{
    [Header("Variables")]
    public int waterCap = 15;
    public int currentWaterLevel;

    [Header("References")]
    public Slider _slider;
    public Image _fill;

    // Start is called before the first frame update
    void Start()
    {
        SetMaxWaterLevel();
    }

    public void FillWaterMeter() // makes the current value the max value
    {
        currentWaterLevel = waterCap;
        SetCurrentWaterLevel();
    }

    public void SetMaxWaterLevel() // sets the value of what the max water level is
    {
        _slider.maxValue = waterCap;
        _slider.value = 0;
    }

    public void SetCurrentWaterLevel() //updates the meter to display what the current integer is 
    {
        _slider.value = currentWaterLevel;
    }
}
