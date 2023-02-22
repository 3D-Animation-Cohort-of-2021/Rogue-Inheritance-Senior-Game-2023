using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFloorGenerator : MonoBehaviour
{
    public Vector2Int floorGridSize;
    public int maxRooms;
    private int numRooms = 0;
    public RoomSetSO roomSet;

    private FloorGrid floorGrid;
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        floorGrid = new FloorGrid(floorGridSize.x, floorGridSize.y);
        GenerateFloor();
    }

    private void GenerateFloor()
    {
        ChooseStartingRoom();
        ChooseRoom();
    }

    private void ChooseStartingRoom()
    {
        Vector2Int startCoord = floorGrid.SelectRoomByRandom();

        if (startCoord.x != -1 && startCoord.y != -1)
        {
            floorGrid.FillRoom(startCoord.x, startCoord.y, roomSet.startRooms.GetRandomObject());
            numRooms++;
        }
        
    }

    private void ChooseRoom()
    {
        Vector2Int roomCoord = floorGrid.SelectRoomByWeight();

        if (roomCoord.x != -1 && roomCoord.y != -1)
        {
            floorGrid.FillRoom(roomCoord.x, roomCoord.y, roomSet.rooms.GetRandomObject());

            numRooms++;

            if (numRooms < maxRooms)
            {
                ChooseRoom();
            }
        }
    }

    private void ChooseEndRoom()
    {

    }

}
