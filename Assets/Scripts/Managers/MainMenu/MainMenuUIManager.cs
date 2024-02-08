using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    // Buttons
    public void Play()
    {
        StateController._stateController.ChangeState(StateController.EGameState.Playing);
    }
}
