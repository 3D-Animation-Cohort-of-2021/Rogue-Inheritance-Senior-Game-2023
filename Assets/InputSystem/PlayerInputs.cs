//Created by: DJ Swiggett
//Last edited by: 
//Purpose: This script uses InputSO's action maps to handle SO's and invoke input events
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public InputSO inputMap;

    private void Awake()
    {
        InputActionMap map = inputMap.map;
        for (int i = 0; i < map.actions.Count; i++)
        {
            if (map.actions[i].name == "Move")
            {
                map.actions[i].started += MoveInput;
                map.actions[i].performed += MoveInput;
                map.actions[i].canceled += MoveInput;
            }
            if (map.actions[i].name == "Dash")
            {
                map.actions[i].started += DashInput;
                map.actions[i].performed += DashInput;
                map.actions[i].canceled += DashInput;
            }
            map.actions[i].Enable();
        }
    }

    /// <summary>
    /// Invokes an event to send data to a controller when move inputs are pressed
    /// </summary>
    private void MoveInput(InputAction.CallbackContext context)
    {
        Debug.Log("Moving");
    }
    /// <summary>
    /// Invokes an event to send data to a controller when dash inputs are pressed
    /// </summary>
    private void DashInput(InputAction.CallbackContext context)
    {
        Debug.Log("Dash");
    }
    
}
