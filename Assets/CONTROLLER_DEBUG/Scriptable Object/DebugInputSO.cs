//Created by: ???
//Last edited by: DJ Swiggett 02/03/23
//Purpose: SO that holds an input map for debugging.
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;
//DJ- I changed the menu name from InputSO to DebugInputSO
[CreateAssetMenu(fileName = "Inputs", menuName = "ScriptableObjects/Inputs/DebugInputSO", order = 0)]
public class DebugInputSO : ScriptableObject 
{

    public InputActionMap map;
}
