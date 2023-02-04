using System;
using System.Collections;
using System.Collections.Generic;
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

  public void SetIndexValue(bool obj, int x, int y)
  {
    grid[x, y] = obj;
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
  ///0=up, 1=right, 2=down, 3=left
  public bool? GetAdjacentStatus(float direction, int currentIndexX, int currentIndexY)
  {
    switch (direction)
    {
      case 0:
      {
        if (currentIndexY + 1 >= grid.GetLength(1))
        {
          Debug.Log("The index requested is out of the grid bounds");
          return null;
        }
        return GetIndexValue(currentIndexX, currentIndexY + 1);
      }
      case 1:
      {
        if (currentIndexX+1>=grid.GetLength(0))
        {
          Debug.Log("The index requested is out of the grid bounds");
          return null;
        }
        return GetIndexValue(currentIndexX + 1, currentIndexY);
      }
      case 2:
      {
        if(currentIndexY<=0)
        {
          Debug.Log("The index requested is out of the grid bounds");
          return null;
        }
        return GetIndexValue(currentIndexX, currentIndexY - 1);
      }
      case 3:
      {
        if(currentIndexX<=0)
        {
          Debug.Log("The index requested is out of the grid bounds");
          return null;
        }
        return GetIndexValue(currentIndexX - 1, currentIndexY);
      }
      default:
      {
        Debug.Log("Invalid direction called for getting adjacent room");
        return null;
      }
    }
  }

  public void SetAdjacentStatus(float direction, int currentIndexX, int currentIndexY, bool statusToSet)
  {
    switch (direction)
    {
      case 0:
      {
        if (currentIndexY + 1 >= grid.GetLength(1))
          Debug.Log("The index requested is out of the grid bounds(Up)");
        else
          SetIndexValue(statusToSet, currentIndexX, currentIndexY + 1);
        break;
      }
      case 1:
      {
        if (currentIndexX+1>=grid.GetLength(0))
          Debug.Log("The index requested is out of the grid bounds(Right)");
        else
          SetIndexValue(statusToSet, currentIndexX + 1, currentIndexY);
        break;
      }
      case 2:
      {
        if(currentIndexY<=0)
          Debug.Log("The index requested is out of the grid bounds(Down)");
        else
         SetIndexValue(statusToSet, currentIndexX, currentIndexY - 1);
        break;
      }
      case 3:
      {
        if(currentIndexX<=0)
          Debug.Log("The index requested is out of the grid bounds(Down)");
        else
          SetIndexValue(statusToSet, currentIndexX - 1, currentIndexY);
        break;
      }
      default:
      {
        Debug.Log("Invalid direction called for getting adjacent room");
        break;
      }
    }
  }

  public bool CoordinateIsOutOfBounds(int x, int y)
  {
    if (x < 0 || x + 1 >= grid.GetLength(0))
      return true;
    if (y < 0 || y + 1 >= grid.GetLength(1))
      return true;
    return false;
  }

  public int[] FindFarthestFrom(int startX, int startY)
  {
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
}
