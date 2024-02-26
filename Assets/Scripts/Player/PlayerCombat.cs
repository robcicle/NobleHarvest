using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Variables")]
    public bool canAttack = true;
    public bool attackSelected = true;
    public int slamRequirement = 8;
    float attackCooldownTimer;
    int slamChargeIndex = 0;
    Collider2D[] enemiesInSlamRadius;

    [Header("References")]
    public GameObject _fireball;
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] SlamAttackChargeMeter _attackChargeMeter;
    [SerializeField] Camera _playerCamera;

    [Header("Values to pass")]
    public float projectileSpeed = 20f;
    // Rigidbody2D _rb;
    Vector2 currentPosition;
    Vector2 mousePosition;

    private void Start()
    {
        _attackChargeMeter.SetMaxCharge(slamRequirement);
    }
    // Update is called once per frame
    void Update()
    {
        //if(slamAttackChargeIndex >= 8)
        //{
        //   Debug.Log("Can slam");
        //}





        //checks if left click is being held down
        // this does the fireball attack
        //  && attackSelected == true
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


            GameObject instantiatedFireball = Instantiate(_fireball, currentPosition, transform.rotation);
            var fireballScript = instantiatedFireball.GetComponent<FireballAttack>();
            fireballScript.force = force; //passes on the force to the instantiated object

            attackCooldownTimer = 0.7f;
            StartCoroutine(AttackCooldown(attackCooldownTimer));

        }

        //checks if right click was used
        // this does the slam attack
        //  && attackSelected == true
        if (Input.GetMouseButtonDown(1) && canAttack == true && slamChargeIndex >= slamRequirement)
        {

            float damage = 50;
            StartCoroutine(ExpandParticles());


            //gets all enemies within a circle of radius 4
            enemiesInSlamRadius = Physics2D.OverlapCircleAll(transform.position, 4f, _enemyLayer);

            //for every enemy inside the radius of the circle get their script, make them take damage and apply force in the opposite direction
            foreach (Collider2D enemy in enemiesInSlamRadius)
            {
                //Debug.Log(enemiesInSlamRadius.Length);
                enemy.GetComponent<EnemyBehaviour>().knockbackHappening = true;
                enemy.GetComponent<EnemyCombat>().TakeDamage(damage);
            }



            slamChargeIndex = 0;
            _attackChargeMeter.UpdateSlamMeter(slamChargeIndex);
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
        for (int i = 0; i < 10; i++)
        {
            _particleSystem.Play();
            particleSystemShape.radius += 0.4f;
            // Debug.Log(particleSystemShape.radius);
            yield return new WaitForSeconds(0.025f);
        }

        //resets the radius of the particle system
        _particleSystem.Stop();
        particleSystemShape.radius = 0;
    }

    //used as a middleman betwen the fireball and slamAttackCharge script
    public void SlamMeterIncrement()
    {

        slamChargeIndex++;
        _attackChargeMeter.UpdateSlamMeter(slamChargeIndex);
    }

    //shows the range of the slam in the editor when gizmos are enabled
    private void OnDrawGizmosSelected()
    {
        if (transform == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(transform.position, 4);
    }
}
