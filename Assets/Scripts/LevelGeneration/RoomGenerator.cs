using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomGenerator : MonoBehaviour
{
    public RoomMatrix gridObject;

    public int finalRoomCount, currentRoomCount;
    private float gridXsize, gridYsize;
    private int[] startCoord, newCoord, currentCoord;
    public int startRange;
    private int newX, newY;
    [SerializeField]private bool currentRoomWillBuild;

    // Start is called before the first frame update
    void Start()
    {
        gridObject.ClearFloor();
        startCoord = new[] {0, 0};
        BuildFirstRoom();
        currentCoord = startCoord;
        BuildRooms();
        gridObject.LayoutFloor();
        int[] testCoord = startCoord;
        for (int i = 0; i < 4; i++)
        {
            testCoord = gridObject.GetAdjacentCoordinate(i, currentCoord);
            Debug.Log(testCoord[0]+" "+testCoord[1]);
        }
        
    }

    public void BuildFirstRoom()
    {
        gridXsize = gridObject.GetGridSize(0);
        startCoord[0] = (int) (Math.Round((gridXsize + 1) / 2));
        startCoord[0] = Random.Range((startCoord[0] - startRange)-1, startCoord[0] + startRange);
        gridYsize = gridObject.GetGridSize(1);
        startCoord[1] = (int) (Math.Round((gridYsize + 1) / 2));
        startCoord[1] = Random.Range((startCoord[1] - startRange)-1, startCoord[1] + startRange);
        if (!gridObject.CoordinateIsOutOfBounds(startCoord))
        {
            gridObject.SetIndexValue(true, startCoord);
        }
        Debug.Log("First room is"+ startCoord);
    }

    private void BuildRooms()
    {
        if (gridObject.OpenBuildSpots(currentCoord) == 0)
            currentRoomWillBuild=false;
        while (currentRoomCount < finalRoomCount && currentRoomWillBuild)
        {
            Debug.Log(currentCoord[0]+" "+currentCoord[1]);
            newCoord = PickRoom(currentCoord);
            gridObject.SetIndexValue(true, newCoord);
            currentRoomCount++;
            currentCoord = newCoord;
            BuildRooms();
        }
    }

    private int[] PickRoom(int[] thisCurrentCoord)
    {
        int[] thisNewCoord = gridObject.GetAdjacentCoordinate(Random.Range(0, 4), thisCurrentCoord);
        if (gridObject.CoordinateIsOutOfBounds(thisNewCoord) || gridObject.GetIndexValue(thisNewCoord))
        {
            thisNewCoord = PickRoom(thisCurrentCoord);
        }
        //Debug.Log(thisNewCoord[0]+" "+thisNewCoord[1]);
        return thisNewCoord;
    }
    
    
}
