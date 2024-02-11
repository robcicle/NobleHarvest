using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    public Slider _slider;
    public Gradient _gradient;
    public Image _fill;
    

    //sets the sliders max value and current value to the max health given
    // this should be called in the start function of whatever is using it 
    public void SetMaxHealth(int maxHealth)
    {
        _slider.maxValue = maxHealth + 20;
        _slider.value = maxHealth + 20; ;
        //sets the colour of the fill to the gradient
        _fill.color = _gradient.Evaluate(1f);
    }

    //correctly adjusts the fill of the slider to what the current health is 
    // this should be called whenever somethings health value changes
    public void UpdateHealthbar(float currentHealth)
    {
        _slider.value = currentHealth + 20;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

}
