using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireballAttack : MonoBehaviour
{
    [Header("Variables")]
    public float timerCountdown = 5;

    [Header("References")]
    public Rigidbody2D _rb;
    public PlayerCombat _playerCombat;
    ParticleSystem _particleSystem;
    SpriteRenderer _spriteRenderer;
    EnemyCombat _enemyCombat;

    [Header("Projectile Stats")]
    public float damage = 20;
    public Vector2 force;

    // Start is called before the first frame update
    void Start()
    {
   
        _particleSystem = GetComponent<ParticleSystem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //move the projectile in the direction given from the playercombat script
        _rb.velocity = force;

    }

    // Update is called once per frame
    void Update()
    {
        //makes the projectile despawn after a set time
        timerCountdown -= Time.deltaTime;
        if(timerCountdown <= 0)
        {
            Explode();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the target hit is an enemy then access its script and damage it
        if(collision.gameObject.tag == "Enemy")
        {
            _enemyCombat = collision.gameObject.GetComponent<EnemyCombat>();
            _enemyCombat.TakeDamage(damage);
            Explode();

        }
    }

    public void Explode()
    {
        
        _rb.velocity = Vector2.zero; // stops the projectile moving
        Destroy(_spriteRenderer); // removes the sprite 
        _particleSystem.Play(); // plays the particle system
        Destroy(gameObject, 0.7f); // destroys the object after 0.7 seconds
    }

}
