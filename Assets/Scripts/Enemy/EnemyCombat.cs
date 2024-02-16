using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Variables")]
    public int maxHealth = 100;
    public float currentHealth;

    [Header("References")]
    //public Rigidbody2D _rb;
    [SerializeField] HealthBar _healthBar;
    EnemyBehaviour _enemyBehaviour;
    [SerializeField] EconomyScreen _economyScreen;

    [Header("Player References")]
    [SerializeField] PlayerMovement _playerMovement;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        _healthBar.SetMaxHealth(maxHealth);
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
        _economyScreen = FindObjectOfType<EconomyScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        _healthBar.UpdateHealthbar(currentHealth);
        //Debug.Log(currentHealth);
        
    }

    public void Die()
    {
        _economyScreen.enemiesKilled++;
        Destroy(gameObject); 
     
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            _playerMovement.Knockback((_enemyBehaviour.forceReference) * 2f);
        }
    }
}
