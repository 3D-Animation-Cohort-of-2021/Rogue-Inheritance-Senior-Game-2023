//Created by: DJ Swiggett
//Last edited by: 
//Purpose: This script uses InputSO's action maps to handle SO's and invoke input events
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public UnityEvent<Vector2> MoveInput, LookInput;
    public UnityEvent DashInput, PrimaryEvent;
    public InputSO inputMap;

    private void Awake()
    {
        InputActionMap map = inputMap.map;
        for (int i = 0; i < map.actions.Count; i++)
        {
            map.actions[i].started += HandleInput;
            map.actions[i].performed += HandleInput;
            map.actions[i].canceled += HandleInput;
            map.actions[i].Enable();
        }
    }

    /// <summary>
    /// Invokes an event to send data to a controller when inputs are pressed
    /// </summary>
    private void HandleInput(InputAction.CallbackContext context)
    {
        if (context.action.name == "Move")
        {
            MoveInput.Invoke(context.ReadValue<Vector2>());
        }
        if (context.action.name == "Look")
        {
            LookInput.Invoke(context.ReadValue<Vector2>());
        }
        if (context.action.name == "Dash" && context.started)
        {
            DashInput.Invoke();
        }
        if (context.action.name == "Interact" && context.started)
        {
            Debug.Log("Interact");
        }
        if (context.action.name == "Back" && context.started)
        {
            Debug.Log("Back");
        }
        if (context.action.name == "PrimaryAttack" && context.started)
        {
            PrimaryEvent.Invoke();
        }
        
    }

}
