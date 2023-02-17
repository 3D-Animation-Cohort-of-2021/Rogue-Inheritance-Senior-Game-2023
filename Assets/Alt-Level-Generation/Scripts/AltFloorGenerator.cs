using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFloorGenerator : MonoBehaviour
{
    public Vector2Int floorGridSize;

    private FloorGrid floorGrid;
    // Start is called before the first frame update
    void Start()
    {
        floorGrid = new FloorGrid(floorGridSize.x, floorGridSize.y);
    }

    private void GenerateFloor()
    {
        CreateStartingRoom();
    }

    private void CreateStartingRoom()
    {

    }
}
