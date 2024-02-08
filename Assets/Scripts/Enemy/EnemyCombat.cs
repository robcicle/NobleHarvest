using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Variables")]
    public int maxHealth = 100;
    public float currentHealth;

    [Header("References")]
    public Rigidbody2D _rb;
    [SerializeField] HealthBar _healthBar;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        _healthBar.SetMaxHealth(maxHealth);
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
        Destroy(gameObject);    
    }
}
