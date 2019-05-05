using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class Unit
    {
        public int uID { get; set; }
        public Player Owner { get; set; }
        public Vector3 UnitPosition { get; set; }
        public string UnitType { get; set; }
        public Vector3 Destination { get; set; }
    }
}
