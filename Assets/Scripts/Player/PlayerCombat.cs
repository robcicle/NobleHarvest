using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Variables")]
    float attackCooldownTimer;
    bool canAttack = true;
    public int slamAttackChargeIndex = 0;
    Collider2D[] enemiesInSlamRadius;

    [Header("References")]
    public GameObject _fireball;
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] LayerMask _enemyLayer;

    [Header("Values to pass")]
    public float projectileSpeed = 20f;
   // Rigidbody2D _rb;
    Vector2 currentPosition;
    Vector2 mousePosition;

    // Update is called once per frame
    void Update()
    {
        if(slamAttackChargeIndex >= 8)
        {
            Debug.Log("Can slam");
        }

   


        //checks if left click is being held down
        // this does the fireball attack
        if (Input.GetMouseButton(0) && canAttack == true)
        {
            //gets the current position of the player and the current mouse cursor position
            //creates a direction and a force in that direction
            //instantiates a fireball prefab with the force value
            //starts a cooldown

            currentPosition = transform.position;
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // gets mouse position in worldspace

            Vector2 direction = (mousePosition - currentPosition).normalized; // gets direction from you to mouse

            Vector2 force = (direction * projectileSpeed); // creates a force in that direction


            GameObject instantiatedFireball = Instantiate(_fireball, currentPosition, transform.rotation, this.transform);
            var fireballScript = instantiatedFireball.GetComponent<FireballAttack>();
            fireballScript.force = force; //passes on the force to the instantiated object

            attackCooldownTimer = 0.8f;
            StartCoroutine(AttackCooldown(attackCooldownTimer));

        }

        //checks if right click was used
        // this does the slam attack
        if(Input.GetMouseButtonDown(1) && canAttack == true && slamAttackChargeIndex >= 8)
        {

            float damage = 50;
            StartCoroutine(ExpandParticles());


            //gets all enemies within a circle of radius 4
            enemiesInSlamRadius = Physics2D.OverlapCircleAll(transform.position, 4f, _enemyLayer);

            //for every enemy inside the radius of the circle get their script, make them take damage and apply force in the opposite direction
            foreach(Collider2D enemy in enemiesInSlamRadius)
            {
                Debug.Log(enemiesInSlamRadius.Length);
                enemy.GetComponent<EnemyBehaviour>().knockbackHappening = true;
                enemy.GetComponent<EnemyCombat>().TakeDamage(damage);
            }



            slamAttackChargeIndex = 0;
            attackCooldownTimer = 1.4f;
            StartCoroutine(AttackCooldown(attackCooldownTimer));
        }
    }



    IEnumerator AttackCooldown(float attackCooldownTimer)
    {
        // this cooldown will be replaced with the animations when those are done
        // when the animation finishes then the player can attack again
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownTimer);
        canAttack = true;

    }

    IEnumerator ExpandParticles()
    {
        var particleSystemShape = _particleSystem.shape;
        //makes the particles expand like a shockwave
        for (int i = 0; i < 20; i++)
        {
            _particleSystem.Play();
            particleSystemShape.radius += 0.2f;
           // Debug.Log(particleSystemShape.radius);
            yield return new WaitForSeconds(0.04f);
        }

        //resets the radius of the particle system
        particleSystemShape.radius = 0;
    }

    //shows the range of the slam in the editor when gizmos are enabled
    private void OnDrawGizmosSelected()
    {
        if(transform == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(transform.position, 4);
    }
}
