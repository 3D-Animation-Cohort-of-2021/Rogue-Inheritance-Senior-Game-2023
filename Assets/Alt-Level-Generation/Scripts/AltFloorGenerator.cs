using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFloorGenerator : MonoBehaviour
{
    public Vector2Int floorGridSize;
    public int maxRooms;
    public float roomSpacing = 2;
    private int numRooms = 0;
    public RoomSetSO roomSet;
    public GameObject hallPrefab;

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
        MakeHalls();
    }

    private void ChooseStartingRoom()
    {
        Vector2Int startCoord = floorGrid.SelectRoomByRandom();

        if (startCoord.x != -1 && startCoord.y != -1)
        {
            floorGrid.FillRoom(startCoord.x, startCoord.y, roomSet.startRooms.GetRandomObject(), roomSpacing);
            numRooms++;
        }
        
    }

    private void ChooseRoom()
    {
        Vector2Int roomCoord = floorGrid.SelectRoomByWeight();

        if (roomCoord.x != -1 && roomCoord.y != -1)
        {
            floorGrid.FillRoom(roomCoord.x, roomCoord.y, roomSet.rooms.GetRandomObject(), roomSpacing);

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

    private void MakeHalls()
    {
        for(int i = 0; i < floorGridSize.x; i++)
        {
            for(int j = 0; j < floorGridSize.y; j++)
            {

                if(floorGrid.CheckForTileNeighbor(i, j, 1))
                {
                    Instantiate(hallPrefab, new Vector3(i * roomSpacing, 0, (j * roomSpacing) + (roomSpacing / 2)), Quaternion.Euler(new Vector3(90, 0, 0)));
                }

                if (floorGrid.CheckForTileNeighbor(i, j, 3))
                {
                    Instantiate(hallPrefab, new Vector3((i * roomSpacing) + (roomSpacing / 2), 0, j * roomSpacing), Quaternion.Euler(new Vector3(0, 0, 90)));
                }

            }
        }
    }

}
