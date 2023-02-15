using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class RoomMatrix : ScriptableObject
{
  public GameObject emptySlot, hallway;
  public AssetList occupiedRooms, endRooms, startRooms, specialRooms, hallways;
  private eRoom[,] grid = new eRoom[12, 12];
  public float layoutSpacing;
  private Vector3 placingVector;
  private List<int[]> possibleRooms = new List<int[]>();
  private List<float> roomWeights = new List<float>();

  public int GetGridSize(int dimension)
  {
    if (dimension < 0 || dimension >= grid.Rank) 
      return 0;
    return grid.GetLength(dimension);
  }

  /// <summary>
/// Fills the matrix with empty room values
/// </summary>
  public void ClearFloor()
  {
    for (int i = 0; i< grid.GetLength(0); i++)
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        grid[i, j] = eRoom.Empty;
      }
  }
  public eRoom GetIndexValue(int[] coord)
  {
    return grid[coord[0], coord[1]];
  }
  /// <summary>
  /// Checks to see if the coordinate is "unoccupied"
  /// </summary>
  /// <param name="coord">Coordinate</param>
  /// <returns>Enum eRoom</returns>
  public bool IndexHasRoom(int[] coord)
  {
    return GetIndexValue(coord) != eRoom.Empty;
  }
  public void SetIndexValue(eRoom obj, int[] coord)
  {
    grid[coord[0], coord[1]] = obj;
  }
  /// <summary>
/// Reads the matrix and places a prefab in the world relative to each location in the matrix that contains a room
/// </summary>
  public void LayoutFloor(bool generateEmptyRooms)
  {
    int[] currentSlot = {0,0};
    for (int i = 0; i < grid.GetLength(0); i++)
    {
      currentSlot[0] = i;
      placingVector.x = i * layoutSpacing;
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        currentSlot[1] = j;
        placingVector.z = j * layoutSpacing;
        switch (GetIndexValue(currentSlot))
        {
          case eRoom.Occupied:
          {
            Instantiate(occupiedRooms.GetRandomObject(), placingVector, quaternion.identity);
            break;
          }
          case eRoom.StartRoom:
          {
            Instantiate(startRooms.GetRandomObject(), placingVector, quaternion.identity);
            break;
          }
          case eRoom.EndRoom:
          {
            Instantiate(endRooms.GetRandomObject(), placingVector, quaternion.identity);
            break;
          }
          case eRoom.SpecialRoom:
          {
            Instantiate(specialRooms.GetRandomObject(), placingVector, quaternion.identity);
            break;
          }
          case eRoom.Empty:
          {
            if(generateEmptyRooms)
              Instantiate(emptySlot, placingVector, quaternion.identity);
            break;
          }
        }
      }
    }
  }
/// <summary>
/// Reads each room in the matrix to see if there needs to be a hallway to the room above or to the right
/// </summary>
  public void LayoutHallways()
  {
    int[] temp = new int[2];
    for (int i = 0; i < grid.GetLength(0); i++)
    {
      temp[0] = i;
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        temp[1] = j;
        if (i<GetGridSize(0)-1&&IndexHasRoom(temp)&&GetAdjacentHasRoom(1, temp))
          Instantiate(hallway, new Vector3((i * layoutSpacing)+(layoutSpacing/2), 0, j * layoutSpacing),
            Quaternion.Euler(new Vector3(0,0,90)));
        if (j<GetGridSize(1)-1&&IndexHasRoom(temp)&&GetAdjacentHasRoom(0, temp))
          Instantiate(hallway, new Vector3(i * layoutSpacing, 0, (j * layoutSpacing)+(layoutSpacing/2)),
            Quaternion.Euler(new Vector3(90, 0, 0)));
      }
    }
    for (int i = 0; i < grid.GetLength(0); i++)
    {
      temp[0] = i;
      for (int j = 0; j < grid.GetLength(1)-1; j++)
      {
        temp[1] = j;
        
      }
    }
  }

/// <summary>
/// Returns the coordinates of the spot in (X) direction
/// </summary>
/// <param name="direction"> 0= up, 1=right, 2=down, 3=left</param>
/// <param name="coord">Current X coordinate</param>
/// <returns>Returns int array</returns>
/// 
  public int[] GetAdjacentCoordinate(int direction, int[] coord)
{
  if (direction>3)
      Debug.Log("Invalid direction "+direction);
  int[] adjCoord = new int[] { };
  switch (direction)
  {
    case 0://up
      adjCoord = new int[] {coord[0], coord[1]+1};
      break;
    case 1://right
      adjCoord = new int[] {coord[0] + 1, coord[1]};
      break;
    case 2://down
      adjCoord = new int[] {coord[0], coord[1] - 1};
      break;
    case 3://left
      adjCoord = new int[] {coord[0] - 1, coord[1]};
      break;
    default:
      coord = null;
      break;
  }
  return coord == null ? null : adjCoord;
  }

/// <summary>
/// Returns the current status of the adjacent coordinate in given direction
/// </summary>
/// <param name="direction">0= up, 1=right, 2=down, 3=left</param>
/// <param name="coord"></param>
/// <returns></returns>
  public bool GetAdjacentHasRoom(int direction, int[] coord)
  {
    return IndexHasRoom(GetAdjacentCoordinate(direction, coord));
  }

public eRoom GetAdjacentRoomStatus(int direction, int[] coord)
{
  return GetIndexValue(GetAdjacentCoordinate(direction, coord));
}
/// <summary>
/// Returns whether or not the requested coordinate adjacent in (x) direction is out of the matrix bounds
/// </summary>
/// <param name="direction"></param>
/// <param name="coord"></param>
/// <returns>boolean</returns>
  public bool AdjacentCoordinateIsOutOfBounds(int direction, int[] coord)
  {
    return CoordinateIsOutOfBounds(GetAdjacentCoordinate(direction, coord));
  }
/// <summary>
/// Checks to see if the given coordinate is out of the grid bounds
/// </summary>
/// <param name="coord"></param>
/// <returns></returns>
  public bool CoordinateIsOutOfBounds(int [] coord)
  {
    if (coord == null)
    {
      Debug.Log("Coordinate is null on boundary check");
      return true;
    }
    //Debug.Log("checking "+coord[0]+" "+coord[1]);
    if (coord[0] < 0 || coord[0] >= grid.GetLength(0))
      return true;
    if (coord[1] < 0 || coord[1] >= grid.GetLength(1))
      return true;
    return false;
  }

/// <summary>
/// Finds the farthest active coordinate in the grid from the given coordinate.
/// </summary>
/// <param name="startCoord"></param>
/// <returns></returns>
  public int[] FindFarthestFrom(int[] startCoord)
  {
    int currentFarthestPath = 0;
    int thisPathLength;
    int[] currentFarthestCoordinate = {0,0};
    for (int i = 0; i< grid.GetLength(0); i++)
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        if (grid[i, j]!=eRoom.Empty)
        {
          thisPathLength = (Math.Abs(startCoord[0] - i) + Math.Abs(startCoord[1] - j));
          if (thisPathLength > currentFarthestPath)
          {
            currentFarthestPath = thisPathLength;
            currentFarthestCoordinate[0] = i;
            currentFarthestCoordinate[1] = j;
          }
          else if (thisPathLength==currentFarthestPath)
            if (Random.Range(0, 2) == 0)
            {
              currentFarthestPath = thisPathLength;
              currentFarthestCoordinate[0] = i;
              currentFarthestCoordinate[1] = j;
            }
        }
      }
    return currentFarthestCoordinate;
  }

public void MarkSpecialRooms()
{
  int[] qCoord = new int[2];
  for (int i = 0; i < grid.GetLength(0); i++)
  {
    qCoord[0] = i;
    for (int j = 0; j < grid.GetLength(1); j++)
    {
      qCoord[1] = j;
      if(IndexHasRoom(qCoord)&& OpenBuildSpots(qCoord)==3)
        SetIndexValue(eRoom.SpecialRoom, qCoord);
    }
  }
}
/// <summary>
/// Returns the number(int) available to build adjacent to the given coordinate.
/// </summary>
/// <param name="coord">Queried coordinate</param>
/// <returns>The number of available rooms to build</returns>
  public int OpenBuildSpots(int[] coord)
  {
    int spots = 0;
    int[] adjCoord;
    for (int i = 0; i < 4; i++)
    {
      adjCoord = GetAdjacentCoordinate(i, coord);
      if (!CoordinateIsOutOfBounds(adjCoord) && !IndexHasRoom(adjCoord))
      {
        spots += 1;
      }
    }
    return spots;
}
/// <summary>
/// Picks randomly from available rooms without using recursion
/// </summary>
/// <param name="coord"></param>
/// <returns>Coordinate (int[])</returns>
public int[] FindRandomRoom(int[] coord)
{
  int[] queryCoord;
  possibleRooms.Clear();
  for (int i = 0; i < 4; i++)
  {
    queryCoord = GetAdjacentCoordinate(i, coord);
    if (!CoordinateIsOutOfBounds(queryCoord) && !IndexHasRoom(queryCoord))
    {
      possibleRooms.Add(queryCoord);
    }
  }
  int chosenIndex = Random.Range(0, possibleRooms.Count - 1);
  //Debug.Log(chosenIndex+" of "+OpenBuildSpots(coord));
  return possibleRooms[chosenIndex];
}
public int[] FindBestRoom(int[] coord)
{
  int[] queryCoord;
  float sumWeights =0f, currentFraction =0f, randomFrac;
  possibleRooms.Clear();
  roomWeights.Clear();
  for (int i = 0; i < 4; i++)//adds possible rooms to list
  {
    queryCoord = GetAdjacentCoordinate(i, coord);
    if (!CoordinateIsOutOfBounds(queryCoord) && !IndexHasRoom(queryCoord))
    {
      possibleRooms.Add(queryCoord);
    }
  }
  foreach (int[] crd in possibleRooms)//Find the sum of the available weights
  {
    sumWeights += OpenBuildSpots(crd);
  }
  randomFrac = Random.Range(0, 1f);
  for (int j = 0; j < possibleRooms.Count; j++)//create matching list for the room weights
  {
    roomWeights.Add(OpenBuildSpots(possibleRooms[j])/sumWeights);
  }
  //Debug.Log(string.Join(", ",roomWeights));
  for (int k = 0; k < roomWeights.Count; k++)//find the random room
  {
    currentFraction += roomWeights[k];
    //Debug.Log("Current sum frac at "+k+" is "+currentFraction);
    if (currentFraction >= randomFrac)
    {
      return possibleRooms[k];
    }
  }

  Debug.Log("did not find best room");
  return possibleRooms[Random.Range(0, possibleRooms.Count)];
}


}
