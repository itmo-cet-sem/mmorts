﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class World
    {
        public Dictionary<int, Unit> Units;
        public World()
        {
            Units = new Dictionary<int, Unit>();
        }
    }
}

