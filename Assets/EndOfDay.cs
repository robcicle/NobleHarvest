using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public class EndOfDay : MonoBehaviour
{

    [Header("Variables")]
    public bool canEndDay;
    bool stoodOnDoor;

    [Header("References")]
    public GamePhase _gamePhase;
    public GameObject _economyScreen;
    public StateController _stateController;
    [SerializeField] GameObject _canInteractText;

    // Start is called before the first frame update
    void Start()
    {
        _canInteractText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && stoodOnDoor == true)
        {
            _stateController.ChangeState(EGameState.Paused); //pauses the game stopping player input
            _economyScreen.SetActive(true);
            _canInteractText.SetActive(false);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("rip bozo");
        if(collision.gameObject.tag == "Player" && canEndDay == true)
        {
            stoodOnDoor = true;
            _canInteractText.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        stoodOnDoor = false;
        _canInteractText.SetActive(false);
    }
}
