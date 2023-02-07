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
  public GameObject occupiedRoom, emptySlot, hallway;
  private bool[,] grid = new bool[9, 9];
  public float layoutSpacing;

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
        grid[i, j] = false;
      }
  }

  public bool GetIndexValue(int[] coord)
  {
    return grid[coord[0], coord[1]];
  }
  
  public void SetIndexValue(bool obj, int[] coord)
  {
    grid[coord[0], coord[1]] = obj;
  }
  
/// <summary>
/// Reads the matrix and places a prefab in the world relative to each location in the matrix that contains a room
/// </summary>
  public void LayoutFloor()
  {
    for (int i = 0; i< grid.GetLength(0); i++)
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        if (grid[i, j])
        {
          //Debug.Log("Room at "+i+", "+j);
          Instantiate(occupiedRoom, new Vector3(i, 0, j)*layoutSpacing, quaternion.identity);
        }
        else
        {
          Instantiate(emptySlot, new Vector3(i, 0, j)*layoutSpacing, quaternion.identity);
        }
      }
  }

  public void LayoutHallways()
  {
    int[] temp = new int[2];
    for (int i = 0; i < grid.GetLength(0); i++)
    {
      temp[0] = i;
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        temp[1] = j;
        if (i<GetGridSize(0)-1&&GetIndexValue(temp)&&GetAdjacentStatus(1, temp))
          Instantiate(hallway, new Vector3((i * layoutSpacing)+(layoutSpacing/2), 0, j * layoutSpacing),
            Quaternion.Euler(new Vector3(0,0,90)));
        if (j<GetGridSize(1)-1&&GetIndexValue(temp)&&GetAdjacentStatus(0, temp))
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
    int[] adjCoord = new int[2];
    if (direction>3)
      Debug.Log("Invalid direction "+direction);
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
  public bool GetAdjacentStatus(int direction, int[] coord)
  {
    return GetIndexValue(GetAdjacentCoordinate(direction, coord));
  }

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
        if (grid[i, j])
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
      if (!CoordinateIsOutOfBounds(adjCoord) && !GetIndexValue(adjCoord))
      {
        spots += 1;
      }
    }
  //Debug.Log(coord[0]+" "+coord[1]+" has "+spots+ "spots open to build");
  return spots;
}


  
}
