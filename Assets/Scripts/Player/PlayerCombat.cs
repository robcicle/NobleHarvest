using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{
    [Header("Variables")]
    bool canAttack = true;

    [Header("References")]
    public GameObject _fireball;

    [Header("Values to pass")]
    public float projectileSpeed = 1500f;
   // Rigidbody2D _rb;
    Vector2 currentPosition;
    Vector2 mousePosition;

    // Update is called once per frame
    void Update()
    {

        //checks if left click was used
        if (Input.GetMouseButtonDown(0) && canAttack == true)
        {

            //gets the current position of the player and the current mouse cursor position
            //creates a direction and a force in that direction
            //instantiates a fireball prefab with the force value
            //starts a cooldown

            currentPosition = transform.position;
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 direction = (mousePosition - currentPosition).normalized;

            Vector2 force = (direction * projectileSpeed * Time.deltaTime) * 2;

            GameObject instantiatedFireball = Instantiate(_fireball, currentPosition, transform.rotation);
            var fireballScript = instantiatedFireball.GetComponent<FireballAttack>();
            fireballScript.force = force;

            StartCoroutine(AttackCooldown());

        }
    }



    IEnumerator AttackCooldown()
    {
        // this cooldown will be replaced with the animations when those are done
        // when the animation finishes then the player can attack again
        canAttack = false;
        yield return new WaitForSeconds(0.8f);
        canAttack = true;

    }
}
