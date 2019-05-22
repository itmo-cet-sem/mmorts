using GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector
{
    public Dictionary<int, Unit> Units;
    public Vector2Int Coordinates;
    public bool IsActive;
    public Sector(Vector2Int coordinates)
    {
        Units = new Dictionary<int, Unit>();
        Coordinates = coordinates;
        IsActive = true;
    }
}
