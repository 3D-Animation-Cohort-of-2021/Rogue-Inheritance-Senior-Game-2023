using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassStatTester : MonoBehaviour
{
    public Vector2Int visualizerGridSize;
    public GameObject statVisualizer;
    public CharacterStatsSO prevCharacter;
    public CharacterStatsSO spouse;

    // Start is called before the first frame update
    void Start()
    {

        VisualizeGrid();
        
    }

    private void VisualizeGrid()
    {
        for(int i = 0; i < visualizerGridSize.x; i++)
        {
            for(int j = 0; j < visualizerGridSize.y; j++)
            {
                GameObject temp = Instantiate(statVisualizer, new Vector3((float)i, 0, (float)j), Quaternion.identity);
                CharacterStatTester statTester = temp.GetComponent<CharacterStatTester>();
                statTester.SetupStatVisualizer(prevCharacter, spouse);
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