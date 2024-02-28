using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float moveSpeed = 50f;

    [Header("Targeting")]
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _targetGameObject;
    public EnemyVariableController _enemyVariableController;

    [Header("List Of Crops")]
    public GameObject cropsListParent;
    public int numberOfCrops;
    int randomCrop;
    public GameObject[] cropList;


    //movement
    [Header("Movement")]
    public Vector2 forceReference;
    Transform currentPosition;
    Transform targetPosition;
    Rigidbody2D _rb;
    Vector2 direction;
    public bool knockbackHappening;

    //sprite changes
    bool isFacingRight;
    float horizontal;

    // CODING NOTES FOR FUTURE IMPROVEMENTS
    // when instantiating crops / crop game objects make sure to add the "Crop" tag to them
    // and add them as a child to "cropsListParent"


    // Start is called before the first frame update
    void Start()
    {

        _enemyVariableController = GetComponentInParent<EnemyVariableController>();
        cropsListParent = GameObject.Find("cropsListParent");
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody2D>();
        currentPosition = gameObject.transform;
  


        //check the number of crops currently present in the game and creates an array of that length
        numberOfCrops = cropsListParent.transform.childCount;
        cropList = new GameObject[numberOfCrops];
        _enemyVariableController.cropsRemaining = numberOfCrops;


        //selects a random number from the length of the array as the target crop
        randomCrop = Random.Range(0, numberOfCrops);
        //Debug.Log(randomCrop);

        // populates the array full of the gameobjects from the heirarchy
        for (int i = 0; i < numberOfCrops; i++)
        {
            cropList[i] = cropsListParent.transform.GetChild(i).gameObject;
        }



        // if there are no crops present then move towards the player instead
        if (cropList == null || numberOfCrops == 0)
        {
            _targetGameObject = _player;
        }
        else
        {
            //selects a random gameobject from the array as the target for the enemy
            _targetGameObject = cropList[randomCrop];
            //Debug.Log(_targetGameObject);
        }


        //if the positions of the gameobjects are not equal then call the function to move the enemy
        targetPosition = _targetGameObject.transform;
        NewTarget();


    }

    //handles physics
    void FixedUpdate()
    {


        //if the target is null then find a new one
        if (_targetGameObject == null || targetPosition.position == null )
        {
            NewTarget();
        }
        else
        {

            currentPosition = gameObject.transform;
            targetPosition = _targetGameObject.transform;
            //Debug.Log(currentPosition.position) ;
            //Debug.Log(targetPosition.position);

            //get both the targets position and the current game objects position to create a vector2, being the direction
            //normalized sets the magnitude of the array to be 1 so the distance doesnt affect the speed
            Vector2 direction = (targetPosition.position - currentPosition.position).normalized;
            //Debug.Log("Direction :" + direction);

            //using the enemies current position and target position
            //calulcate the vector distance between them and apply force to move in that direction
            if (knockbackHappening == false)
            {
                Vector2 force = direction * moveSpeed * Time.deltaTime;
                forceReference = force;

                //Debug.Log("Force" + force);

                //if the enemy isnt on the crop then apply a force in that direction
                if (currentPosition.position != targetPosition.position)
                {
                    _rb.velocity = force;
                }

                //if the enemy is on the crop, stop moving
                if (Vector2.Distance(currentPosition.position, targetPosition.position) < 0.1f && _targetGameObject != _player)
                {
                    _rb.velocity = new Vector2(0, 0);
                }
            }

            //apply knockback here
            else
            {
                direction = (_player.transform.position - currentPosition.position).normalized;
                Vector2 force = (-40 * direction) * moveSpeed * Time.deltaTime;
                _rb.velocity = force;
                StartCoroutine(StopKnockback());
            }

        }


    }

    //handles pathfinding
    private void Update()
    {

        horizontal = _rb.velocity.x; // checks the horizontal speed of the enemy
        FlipSprite(); // flips the sprite to the correct direction 


        // ensures that the index of crops remaining is correct
        numberOfCrops = _enemyVariableController.cropsRemaining;

        //updates the array and removes any null values accordingly
        cropList = new GameObject[numberOfCrops];
        for (int i = 0; i < numberOfCrops; i++)
        {
            cropList[i] = cropsListParent.transform.GetChild(i).gameObject;
            if (cropList[i] == null)
            {
                RemoveElement(ref cropList, i);
            }
        }
        //if the target / transform is null select a new one
        if (_targetGameObject == null || _targetGameObject.transform == null)
        {
            NewTarget();
        }

        //if the array is empty / index is 0 then target the player
        if (cropList == null || numberOfCrops == 0)
        {
            _targetGameObject = _player;
            targetPosition = _targetGameObject.transform;
        }





        //
        //DEBUG SECTION
        //


        //randomly choose a different crop in the array
       // if (Input.GetKeyDown(KeyCode.E))
        //{
        //    randomCrop = Random.Range(0, numberOfCrops);
        //    _targetGameObject = cropList[randomCrop];
        //    Debug.Log(_targetGameObject);
       //}



        // test if the enemy moves, they still path correctly to the crop
        // see if knockback could be implemented
        //if (Input.GetKeyDown(KeyCode.P))
       // {
        //    currentPosition.position += Vector3.down * 3;
        //}
    }


    public void DestroyCrop(GameObject cropReference)
    {
       // _targetGameObject.SetActive(false);
       //ensures all enemies know a crop has been killed
        Destroy(cropReference);
        _enemyVariableController.UpdateEnemytargets();



      


        //when there are no more plants left to attack then it moves onto the player
        if (cropList == null || numberOfCrops == 0)
        {
            _targetGameObject = _player;
        }


    }
    public void NewTarget()
    {

        // selects a new target from the updated array
        // this ensures that the chosen target cant be a null value
        // when there are no more crops left to attack then it moves onto the player
        if (cropList != null && numberOfCrops > 0)
        {
            StartCoroutine(WaitForArrayUpdate());
        }
        
        if(numberOfCrops == 0)
        {
            _targetGameObject = _player;
        }





    }

    public void ArrayCleanup()
    {
        //checks if each element is null and removes them
        for (int i = 0; i < numberOfCrops; i++)
        {
            cropList[i] = cropsListParent.transform.GetChild(i).gameObject;
            if (cropList[i] == null)
            {
                RemoveElement(ref cropList, i);
            }
        }

        //cleans up the array then select a new target if nothing is being targeted
        if(_targetGameObject == null || _targetGameObject.transform == null)
        {
            NewTarget();
        }
 
    }

    // "removes" elements by re-writing a copy of the arrray minus that element
    public static void RemoveElement<T>(ref T[] arr, int index)
    {

        // to call use RemoveElement(ref *arrayname*, *array index*)
        for(int a = index; a < arr.Length - 1; a++)
        {
            //move elements down the array to close up the gap at the index
            //does this by changing the elemenets index
            arr[a] = arr[a + 1];
        }

        //resize the array so that the length matches the number of elements
        Array.Resize(ref arr, arr.Length - 1);


    }

    //waits for the array to update so that when checking the enemies there arent any null values that cause errors
    IEnumerator WaitForArrayUpdate()
    {
        yield return new WaitForEndOfFrame();
        randomCrop = Random.Range(0, numberOfCrops);
        _targetGameObject = cropList[randomCrop];

        // recalibrates the position of the target
        if (_targetGameObject == null)
        {
             NewTarget() ;
        }
        else
        {
            currentPosition = gameObject.transform;
            targetPosition = _targetGameObject.transform;
    
        }
    }


    //
    // not currrently using this
    //
    private bool IsCropArrayEmpty()
    {
        //checks to see the length of the array and if it void or not
        if (cropList == null || cropList.Length == 0)
        {
            return true;
        }
        //checks each point in the array to check for void elements
        if(cropList != null)
        {
            for (int i = 0; i < cropList.Length; i++)
            {
                if (cropList[i] != null)
                {
                    return false;
                }

            }

        }

        return false;
    }

    IEnumerator StopKnockback()
    { 

        yield return new WaitForSeconds(0.05f);
        knockbackHappening = false;
    }

    public void FlipSprite()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {

            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
