//Created by: Marshall Krueger
//Last edited by: Marshall Krueger 01/30/23
//Purpose: This is a sample script
using UnityEngine;

public class SampleScript : MonoBehaviour
{

    /// <summary>
    /// Purpose: Check if character is weak to a specified element type
    /// </summary>
    /// <param name="sample">any string</param>
    /// <returns>true if the string in sample contains "sample"</returns>
    public bool CheckSample(string sample){

        if(sample == "sample")
        {
            return true;
        }

        return false;
    }
}
