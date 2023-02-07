using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class RoomMatrix : ScriptableObject
{
  public GameObject occupiedRoom, emptySlot;
  private bool[,] grid = new bool[7, 7];
  public float layoutSpacing;

  public int GetGridSize(int dimension)
  {
    if (dimension < 0 || dimension >= grid.Rank) 
      return 0;
    return grid.GetLength(dimension);
  }
  public void ClearFloor()
  {
    for (int i = 0; i< grid.GetLength(0); i++)
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        grid[i, j] = false;
      }
  }

  public bool GetIndexValue(int x, int y)
  {
    return grid[x, y];
  }
  public bool GetIndexValue(int[] coord)
  {
    return grid[coord[0], coord[1]];
  }
  public void SetIndexValue(bool obj, int x, int y)
  {
    grid[x, y] = obj;
  }
  public void SetIndexValue(bool obj, int[] coord)
  {
    grid[coord[0], coord[1]] = obj;
  }

  public void LayoutFloor()
  {
    for (int i = 0; i< grid.GetLength(0); i++)
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        if (grid[i, j])
        {
          Debug.Log("Room at "+i+", "+j);
          Instantiate(occupiedRoom, new Vector3(i, 0, j)*layoutSpacing, quaternion.identity);
        }
        else
        {
          Instantiate(emptySlot, new Vector3(i, 0, j)*layoutSpacing, quaternion.identity);
        }
      }
  }
  /// <summary>
  /// Returns the coordinates of the spot in (X) direction
  /// </summary>
  /// <param name="direction"> 0= up, 1=right, 2=down, 3=left</param>
  /// <param name="xCoord">Current X coordinate</param>
  /// <param name="yCoord">Current Y coordinate</param>
  /// <returns>Returns</returns>
  /// 
  public int[] GetAdjacentCoordinate(int direction, int xCoord, int yCoord)
  {//need to convert to array input
    int[] coord;
    switch (direction)
    {
      case 0:
        coord = new[] { xCoord, yCoord + 1 };
        break;
      case 1:
        coord = new[] { xCoord + 1 , yCoord};
        break;
      case 2:
        coord = new[] { xCoord, yCoord - 1 };
        break;
      case 3:
        coord = new[] { xCoord - 1, yCoord};
        break;
      default:
        coord = null;
        break;
    }
    if (coord == null || CoordinateIsOutOfBounds(coord[0], coord[1]))
      return null;
    return coord;
  }

  public bool GetAdjacentStatus(int direction, int xCoord, int yCoord)
  {
    return GetIndexValue(GetAdjacentCoordinate(direction, xCoord, yCoord));
  }
  public bool GetAdjacentStatus(int direction, int[] coord)
  {
    return GetIndexValue(GetAdjacentCoordinate(direction, coord[0], coord[1]));
  }
  
  public bool CoordinateIsOutOfBounds(int x, int y)
  {//need to convert to array input
    if (x < 0 || x + 1 >= grid.GetLength(0))
      return true;
    if (y < 0 || y + 1 >= grid.GetLength(1))
      return true;
    return false;
  }

  public int[] FindFarthestFrom(int startX, int startY)
  {//need to convert to array input
    int currentFarthestPath = 0;
    int thisPathLength;
    int[] currentFarthestCoordinate = new int[]{0,0};
    for (int i = 0; i< grid.GetLength(0); i++)
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        if (grid[i, j])
        {
          thisPathLength = (Math.Abs(startX - i) + Math.Abs(startY - j));
          if (thisPathLength > currentFarthestPath)
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
  /// Get the number or weights of filled rooms that are adjacent to the given coordinate
  /// </summary>
  /// <param name="xCoord"></param>
  /// <param name="yCoord"></param>
  /// <returns>Room weight</returns>
  public float GetCrowdedWeight(int xCoord, int yCoord)
  {//need to convert to array input
    float weight = 0;
    for (int i = 0; i < 4; i++)
    {
      if (GetIndexValue(GetAdjacentCoordinate(i, xCoord, yCoord))) 
      {
        weight += 1;
      }
    }
    return weight;
  }

  
}
