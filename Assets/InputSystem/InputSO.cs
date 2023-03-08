//Created by: DJ Swiggett
//Last edited by: 
//Purpose: SO I copied from debugSO because I didn't want our input map so to be called debugSO
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "Inputs", menuName = "ScriptableObjects/Inputs/InputSO", order = 0)]
public class InputSO : ScriptableObject 
{

    public InputActionMap map;
}
