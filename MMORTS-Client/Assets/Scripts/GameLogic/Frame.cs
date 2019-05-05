using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class Frame
    {
        public string Name { get; set; }
        public string GameName { get; set; }
        public int Size { get; set; }
        public int ComponentsAvalible
        {
            get
            {
                int componentsSum = MovementComponents + ToolsComponents + CoreComponents + ArmorComponents;
                return componentsSum;
            }
        }
        public int MovementComponents { get; set; }
        public int ArmorComponents { get; set; }
        public int CoreComponents { get; set; }
        public int ToolsComponents { get; set; }
        public Frame(string key)
        {
            switch (key)
            {
                case "HeavyUnit":
                    GameName = "Heavy Frame";
                    Size = 1;
                    MovementComponents = 1;
                    ArmorComponents = 2;
                    CoreComponents = 1;
                    ToolsComponents = 4;
                    break;
                case "LightUnit":
                    GameName = "Light Frame";
                    Size = 1;
                    MovementComponents = 1;
                    ArmorComponents = 1;
                    CoreComponents = 1;
                    ToolsComponents = 2;
                    break;
            }
            Name = key;
        }
    }
}
