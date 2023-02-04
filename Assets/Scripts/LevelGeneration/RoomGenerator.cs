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
    private int startX, startY;
    public int startRange;
    private int newX, newY;
    private bool currentRoomWillBuild;

    // Start is called before the first frame update
    void Start()
    {
        gridObject.ClearFloor();
        BuildFirstRoom();
        gridObject.LayoutFloor();
        
        Debug.Log("The value above 3,4 is "+gridObject.GetAdjacentStatus(0, 3,4));
    }

    public void BuildFirstRoom()
    {
        gridXsize = gridObject.GetGridSize(0);
        startX = (int) (Math.Round(gridXsize / 2));
        startX = Random.Range((startX - startRange)-1, startX + startRange);
        gridYsize = gridObject.GetGridSize(1);
        startY = (int) (Math.Round(gridYsize / 2));
        startY = Random.Range((startY - startRange)-1, startY + startRange);
        if (!gridObject.CoordinateIsOutOfBounds(startX, startY))
        {
            gridObject.SetIndexValue(true, startX, startY);
        }
    }

   /* private void BuildRoom(int thisX, int thisY)
    {
        while (currentRoomCount < finalRoomCount && currentRoomWillBuild)
        {
            //Decide if and where to build room
                
            //Use coordinate of new room to call function again
            BuildRoom(newX, newY);
        }
    }*/
    
    
}
