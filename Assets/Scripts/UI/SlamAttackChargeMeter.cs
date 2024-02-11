using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlamAttackChargeMeter : MonoBehaviour
{
    [Header("References")]
    public Slider _slider;
    public Gradient _gradient;
    public Image _fill;
    public ParticleSystem _particleSystem;

    // maxCharge


    //sets the sliders max value and current value to the max health given
    // this should be called in the start function of whatever is using it 
    public void SetMaxCharge(int slamRequirement)
    {
        _slider.maxValue = slamRequirement + 1;
        _slider.value = _slider.minValue; 
        //sets the colour of the fill to the gradient
        _fill.color = _gradient.Evaluate(1f);
    }

    //correctly adjusts the fill of the slider to what the current health is 
    // this should be called whenever somethings health value changes
    public void UpdateSlamMeter(int CurrentSlamIndex)
    {
        switch (CurrentSlamIndex)
        {
            case 0:
                _particleSystem.Stop(); // stops the particles once the player has used the slam
                break;
            case >= 8:
                CurrentSlamIndex = 8;
                _particleSystem.Play(); //plays the particle system to let the player know that the slam is ready
                break;
        }


        _slider.value = CurrentSlamIndex + 1;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue); //update the charge meter


        //if (CurrentSlamIndex >= 8 )
        //{
        //    CurrentSlamIndex = 8;
        //    _particleSystem.Play();
        //}


       // if(CurrentSlamIndex == 0)
       // {
       //     _particleSystem.Stop();
       // }
 
    }




}