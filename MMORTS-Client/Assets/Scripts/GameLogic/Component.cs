using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public enum ComponentPositions { Movement, Tool, Armor, Core }
    public class Component
    {
        public string Name { get; set; }
        public ComponentPositions ComponentPosition { get; set; }
        public float Weight { get; set; }
        public Sprite Image { get; set; }
        public Component(string templateKey)
        {
            switch (templateKey)
            {
                case "Wheels":
                    ComponentPosition = ComponentPositions.Movement;
                    Weight = 5;
                    Image = Resources.LoadAll(@"Textures\units")[1] as Sprite;
                    break;
                case "Legs":
                    ComponentPosition = ComponentPositions.Movement;
                    Weight = 15;
                    Image = Resources.LoadAll(@"Textures\units")[2] as Sprite;
                    break;
                case "Armor":
                    ComponentPosition = ComponentPositions.Armor;
                    Weight = 50;
                    Image = Resources.LoadAll(@"Textures\units")[4] as Sprite;
                    break;
                case "AICore":
                    ComponentPosition = ComponentPositions.Core;
                    Weight = 10;
                    Image = Resources.LoadAll(@"Textures\units")[5] as Sprite;
                    break;
                case "WarLaser":
                    ComponentPosition = ComponentPositions.Tool;
                    Weight = 50;
                    Image = Resources.LoadAll(@"Textures\units")[6] as Sprite;
                    break;
                case "Drill":
                    ComponentPosition = ComponentPositions.Tool;
                    Weight = 100;
                    Image = Resources.LoadAll(@"Textures\units")[7] as Sprite;
                    break;
                default:
                    Weight = 0;
                    break;
            }
            Name = templateKey;
        }
    }
}
