using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class RoomData
{
    private Vector2Int gridCoordinate;
    private bool occupied = false;
    private Neighbors neighbors;
    private int weight;
    private exRoom roomType = exRoom.Room;
    private GameObject roomObject;

    public bool IsOccupied { get { return occupied; } }
    public int Weight { get { return weight; } }

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

    public bool OccupyRoom(GameObject gameObject)
    {
        occupied = true;
        weight = 0;

        UpdateNeighbors();


        return false;
    }



    public bool UpdateWeight()
    {
        int occupiedNeighbors = neighbors.GetNumberOfOccupiedNeighbors();

        if(occupiedNeighbors > 0)
        {
            weight = 4 - occupiedNeighbors;
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
