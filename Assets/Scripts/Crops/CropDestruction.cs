using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    [Header("References")]
    public EnemyVariableController _enemyVariableController;
    public EnemyBehaviour _enemyBehaviour;
    //timer
    float timeRemaining = 6f;
    bool startTimer;
    bool isInteracting;
    GameObject thisCrop;
    // Start is called before the first frame update
    void Start()
    {
        thisCrop = gameObject;
       
    }

    // Update is called once per frame
    void Update()
    {
        // a 3 second timer for when an enemy is stood on a plant
        // it destroys it
        // this may need changing to plant wiltering when those are added
        if(startTimer == true)
        {
            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
                //Debug.Log(timeRemaining);
            }
            else
            {
                _enemyBehaviour.DestroyCrop(thisCrop);
            }
           
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
              //isInteracting = true;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        isInteracting = true;
        // if the thing colliding is an enemy then get which enemy has collided
        // as a reference
        if (collision.gameObject.tag == "Enemy" && isInteracting == true)
        {
            isInteracting = true;
            _enemyBehaviour = collision.gameObject.GetComponent<EnemyBehaviour>();
            // Debug.Log(timeRemaining);
            startTimer = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        // checks to see if the enemy has left and stop the crops being destroyed
        if (collision.gameObject.tag == "Enemy")
        {
        
            
            isInteracting = false;
           // Debug.Log(timeRemaining);
            startTimer = false;
        }
    }

}
