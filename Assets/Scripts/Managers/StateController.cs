using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateController : MonoBehaviour
{
    public static StateController _stateController = null;

    // All of the potential states.
    public enum EGameState
    {
        MainMenu,
        Playing,
        Paused
    }
    // Keep track of current state
    [SerializeField]
    private EGameState _eGameState = EGameState.MainMenu;

    private void Awake()
    {
        // Assert if there is already a controller.
        Debug.Assert(_stateController == null, 
            "Multiple instances of singleton has already been created", 
            this.gameObject
            );

        // Handle of the first controller created.
        _stateController = this;
    }

    private void Start()
    {
        // If we have more than just management loaded on startup
        // lets make sure that we unload the extra ones so that
        // they can be created and managed asynchronously.
        if (SceneManager.sceneCount > 1)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i) != SceneManager.GetSceneByName("Management"))
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }

        // Ensure we are in the current state,
        // and do all the required state intstructions.
        ChangeState(_eGameState);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            ChangeState(EGameState.Paused);

        switch(_eGameState)
        {
            // Game is on main menu.
            case EGameState.MainMenu:
                break;
            // Game is playing.
            case EGameState.Playing:
                break;
            // Game is paused.
            case EGameState.Paused:
                break;
            // Capure any out of state errors.
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ChangeState(EGameState eGameState)
    {
        Debug.Log("~Changing State to - " + eGameState);

        switch (eGameState)
        {
            // Game is on main menu
            case EGameState.MainMenu:
                if (_eGameState == EGameState.Playing || _eGameState == EGameState.Paused)
                {
                    SceneManager.UnloadSceneAsync("Game");
                    SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
                }
                else
                {
                    SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
                }
                Time.timeScale = 1.0f;
                break;
            // Game is playing
            case EGameState.Playing:
                if (_eGameState == EGameState.MainMenu)
                {
                    SceneManager.UnloadSceneAsync("MainMenu");
                    SceneManager.LoadScene("Game", LoadSceneMode.Additive);
                }
                else
                {
                    SceneManager.LoadScene("Game", LoadSceneMode.Additive);
                }
                Time.timeScale = 1.0f;
                break;
            // Game is paused
            case EGameState.Paused:
                if (_eGameState == EGameState.MainMenu)
                {
                    Debug.LogError("STATE-ERROR: Shouldn't be able to transition from MAINMENU to PAUSED");
                }

                Time.timeScale = 0.0f;
                break;
            // Capure any out of state errors
            default:
                throw new ArgumentOutOfRangeException();
        }

        // Actually set the state
        _eGameState = eGameState;
    }

    public EGameState GetState()
    {
        return _eGameState;
    }

    public bool IfStateIsActive(EGameState _state)
    {
        if (_eGameState == _state)
            return true;
        else
            return false;
    }
}
