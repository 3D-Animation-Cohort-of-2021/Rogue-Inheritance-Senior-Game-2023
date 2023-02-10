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
    private int newX, newY, currentStepsFromStart, mostStepsFromStart;
    [SerializeField] private int backTrackDelay;
    [SerializeField]private bool currentRoomWillBuild, buildingRooms;
    public float chanceToBackTrack;
    void Start()
    {
        gridObject.ClearFloor();
        startCoord = new[] {0, 0};
        BuildFirstRoom();
        currentCoord = startCoord;
        BuildRooms(startCoord);
        gridObject.SetIndexValue(eRoom.EndRoom, gridObject.FindFarthestFrom(startCoord));
        gridObject.LayoutFloor();
        gridObject.LayoutHallways();
    }
/// <summary>
/// Picks and assigns the first room of the matrix
/// </summary>
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
            gridObject.SetIndexValue(eRoom.StartRoom, startCoord);
        }
        Debug.Log("First room is"+ startCoord[0]+""+startCoord[1]);
    }
/// <summary>
/// Recursive Function: Decides if the current room will build and where to build it.
/// </summary>
    private void BuildRooms(int[] thisLayerCoord)
{
        if (gridObject.OpenBuildSpots(currentCoord) == 0)
        {
            Debug.Log(currentCoord[0]+" "+currentCoord[1]+"Could not build");
            return;
        }
        while (currentRoomCount < finalRoomCount)
        {
            currentCoord = thisLayerCoord;
            if (gridObject.OpenBuildSpots(currentCoord) == 0)
            {
                Debug.Log(currentCoord[0]+" "+currentCoord[1]+"Has no open spots to build"); 
                return;
            }
            if(currentRoomCount>backTrackDelay)
                if (Random.Range(0, 100) < chanceToBackTrack)
                {
                    Debug.Log(currentCoord[0]+" "+currentCoord[1]+"Chose to not build"); 
                    return;
                }
            newCoord = gridObject.FindBestRoom(currentCoord);
            gridObject.SetIndexValue(eRoom.Occupied, newCoord);
            currentRoomCount++;
            currentCoord = newCoord;
            Debug.Log(currentCoord[0]+" "+currentCoord[1]);
            BuildRooms(currentCoord);
        }
    }
/// <summary>
/// Called by BuildRooms(): takes the current coordinate and returns the new coordinate to build in.
/// </summary>
/// <param name="thisCurrentCoord">The current coordinate</param>
/// <returns>The new coordinate that was chosen</returns>
    private int[] PickRoom(int[] thisCurrentCoord)
    {
        int[] thisNewCoord = gridObject.GetAdjacentCoordinate(Random.Range(0, 4), thisCurrentCoord);
        if (gridObject.CoordinateIsOutOfBounds(thisNewCoord) || gridObject.IndexHasRoom(thisNewCoord))
        {
            thisNewCoord = PickRoom(thisCurrentCoord);
        }
        //Debug.Log(thisNewCoord[0]+" "+thisNewCoord[1]);
        return thisNewCoord;
    }

}
