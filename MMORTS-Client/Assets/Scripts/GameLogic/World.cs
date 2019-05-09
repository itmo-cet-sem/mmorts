using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class World
    {
        public UnitType TempSelectedType;
        public Dictionary<Vector2Int,Sector> Sectors;
        public World()
        {
            Sectors = new Dictionary<Vector2Int, Sector>();
        }
    }
}

