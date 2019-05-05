using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class World
    {
        public Dictionary<int, Unit> Units;
        public UnitType TempSelectedType;
        public World()
        {
            Units = new Dictionary<int, Unit>();
        }
    }
}

