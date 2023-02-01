using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMatrix : ScriptableObject
{
  public GameObject[,] grid = new GameObject[5, 5];

  public void ClearFloor()
  {
    for (int i = 0; i< grid.GetLength(0); i++)
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        grid[i, j] = null;
      }
  }
}
