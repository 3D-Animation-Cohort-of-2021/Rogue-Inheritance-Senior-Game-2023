using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class RoomData
{
    private Vector2Int gridCoordinate;
    private bool occupied = false;
    private Neighbors neighbors;
    private int weight = 0;
    private exRoom roomType = exRoom.Room;
    private GameObject roomObject;


    public Vector2Int GridCoordinate
    {
        get { return gridCoordinate; }
    }
    public bool IsOccupied { get { return occupied; } }
    public int Weight { get { return weight; } }
    public exRoom RoomType { get { return roomType; } }

    public RoomData(int xCoord, int zCoord)
    {
        weight = 0;
        occupied = false;
        neighbors = new Neighbors();
        gridCoordinate = new Vector2Int(xCoord, zCoord);
    }

    public bool SetNeighbors(RoomData north, RoomData south, RoomData east, RoomData west)
    {
        neighbors.SetNeighbors(north, south, east, west);

        return false;
    }

    public bool OccupyRoom(GameObject gameObject, float roomSpacing)
    {
        occupied = true;
        weight = 0;
        roomObject = GameObject.Instantiate(gameObject, new Vector3((float)gridCoordinate.x * roomSpacing, 0f, (float)gridCoordinate.y * roomSpacing), Quaternion.identity);
        UpdateNeighbors();


        return false;
    }



    public bool UpdateWeight()
    {
        int occupiedNeighbors = neighbors.GetNumberOfOccupiedNeighbors();

        if(occupiedNeighbors > 0 && !occupied)
        {
            weight = (int)Mathf.Pow(4 - (float)occupiedNeighbors, 2);
            return true;
        }
        else
        {
            weight = 0;
            return true;
        }

        return false;
    }

    public bool UpdateNeighbors()
    {
        for(int i = 0; i < neighbors.size; i++)
        {
            if (neighbors.GetNeighborByIndex(i) != null)
            {
                neighbors.GetNeighborByIndex(i).UpdateWeight();
            }
        }

        return false;
    }

    public bool CheckForNeighbor(int index)
    {
        if(occupied && neighbors.GetNeighborByIndex(index) != null && neighbors.GetNeighborByIndex(index).IsOccupied)
        {
            return true;
        }
        return false;
    }


}

public struct Neighbors
{
    private RoomData north;
    private RoomData south;
    private RoomData east;
    private RoomData west;
    public int size;

    public bool SetNeighbors(RoomData north, RoomData south, RoomData east, RoomData west)
    {
        this.north = north;
        this.south = south;
        this.east = east;
        this.west = west;

        size = 4;

        return false;
    }

    public int GetNumberOfOccupiedNeighbors()
    {
        int numOccupied = 0;

        for(int i = 0; i < size; i++)
        {
            if (GetNeighborByIndex(i) != null && GetNeighborByIndex(i).IsOccupied)
            {
                numOccupied++;
            }
        }

        return numOccupied;
    }

    public RoomData GetNeighborByIndex(int index)
    {
        switch (index)
        {
            case 0: return north;
            case 1: return south;
            case 2: return east;
            case 3: return west;
            default: return null;
        }
    }
}
