using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGrid
{
    private RoomData[,] floorRooms;

    public FloorGrid(int gridWidth, int gridHeight)
    {
        floorRooms = new RoomData[gridWidth, gridHeight];

        SetupRooms();

        EstablishNeighbors();
        
    }

    private void SetupRooms()
    {
        for (int i = 0; i < floorRooms.GetLength(0); i++)
        {
            for (int j = 0; j < floorRooms.GetLength(1); j++)
            {
                floorRooms[i, j] = new RoomData(i, j);
            }
        }
    }

    private void EstablishNeighbors()
    {
        for (int i = 0; i < floorRooms.GetLength(0); i++)
        {
            for (int j = 0; j < floorRooms.GetLength(1); j++)
            {
                RoomData north = null;
                RoomData south = null;
                RoomData east = null;
                RoomData west = null;

                if (j > 0)
                {
                    north = floorRooms[i, j - 1];
                }

                if (j < floorRooms.GetLength(1) - 1)
                {
                    south = floorRooms[i, j + 1];
                }

                if (i > 0)
                {
                    east = floorRooms[i - 1, j];
                }

                if (i < floorRooms.GetLength(0) - 1)
                {
                    west = floorRooms[i + 1, j];
                }

                floorRooms[i, j].SetNeighbors(north, south, east, west);

            }
        }
    }

    public void ClearFloor()
    {
        SetupRooms();
        EstablishNeighbors();
    }

    public RoomData GetRoom(int xCoord, int zCoord)
    {
        return floorRooms[xCoord, zCoord];
    }

    public void FillRoom(int xCoord, int zCoord, GameObject gameObject)
    {
        floorRooms[xCoord, zCoord].OccupyRoom(gameObject);
    }

    public Vector2Int SelectRoomByRandom()
    {
        int randX = Random.Range(0, floorRooms.GetLength(0));
        int randZ = Random.Range(0, floorRooms.GetLength(1));

        if(!floorRooms[randX, randZ].IsOccupied)
        {
            return new Vector2Int(randX, randZ);
        }

        return new Vector2Int(-1, -1);
    }

    
}
