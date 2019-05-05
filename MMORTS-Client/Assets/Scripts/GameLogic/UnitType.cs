using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class UnitType
    {
        public string Name { get; set; }
        public Frame UnitFrame { get; set; }
        public int Size { get; set; }
        public Sprite Image { get; set; }
        public List<Component> Components;
        public UnitType(Frame frame, List<Component> components, string name)
        {
            UnitFrame = frame;
            Size = 1;
            Components = components;
            Name = name;
        }
        private void createSprite()
        {
            Texture2D sprite = new Texture2D(128, 128);
            for (int i = 0; i < Components.Count; i++)
            {
                
            }
            Image = Sprite.Create(sprite, new Rect(), Vector2.zero);
        }
    }
}
