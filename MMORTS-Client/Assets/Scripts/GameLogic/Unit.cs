using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public enum UnitTypes { Circle, Square }
    public class Unit
    {
        public int uID { get; set; }
        public Player Owner { get; set; }
        public Vector3 UnitPosition { get; set; }
        public UnitTypes UnitType { get; set; }
        public Vector3 Destanation { get; set; }
    }
}
