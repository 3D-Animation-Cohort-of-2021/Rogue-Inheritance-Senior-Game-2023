using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassStatTester : MonoBehaviour
{
    public Vector2Int visualizerGridSize;
    public GameObject statVisualizer;
    public CharacterStatsSO prevCharacter;
    public CharacterStatsSO spouse;

    private int[] highestStat = new int[4];

    // Start is called before the first frame update
    void Start()
    {

        VisualizeGrid();

        PrintStatDistribution();
        
    }

    private void PrintStatDistribution()
    {
        Debug.Log($"Strength Characters: {highestStat[0]}");
        Debug.Log($"Agility Characters: {highestStat[1]}");
        Debug.Log($"Wit Characters: {highestStat[2]}");
        Debug.Log($"Balanced Characters: {highestStat[3]}");
    }

    private void VisualizeGrid()
    {
        for(int i = 0; i < visualizerGridSize.x; i++)
        {
            for(int j = 0; j < visualizerGridSize.y; j++)
            {
                GameObject temp = Instantiate(statVisualizer, new Vector3((float)i - visualizerGridSize.x / 2, 0, (float)j - visualizerGridSize.y / 2), Quaternion.identity);
                CharacterStatTester statTester = temp.GetComponent<CharacterStatTester>();
                statTester.SetupStatVisualizer(prevCharacter, spouse);
                highestStat[statTester.GetHighestStat()]++;
            }
        }
    }

    
}

[System.Serializable]
public struct Vector2Int
{
    public int x;
    public int y;
}

[System.Serializable]
public struct Vector3Int
{
    public int x;
    public int y;
    public int z;
}