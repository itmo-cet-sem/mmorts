using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    static public class GameManager
    {
        public static Player CurrentPlayer;
        public static List<Player> Players;
        public static World CurrentWorld;
        public static int LastState;
        public static Dictionary<string, UnitType> UnitTypes;
        public static Dictionary<string, Component> Components;
        public static Dictionary<string, Frame> Frames;
        public static void onStart()
        {
            UnitTypes = new Dictionary<string, UnitType>();
            createComponents();
            createFrames();
        }
        private static void createComponents()
        {
            if (Components != null)
            {
                Components.Clear();
            }
            Components = new Dictionary<string, Component>();
            Components.Add("Wheels", new Component("Wheels"));
            Components.Add("Legs", new Component("Legs"));
            Components.Add("Armor", new Component("Armor"));
            Components.Add("AICore", new Component("AICore"));
            Components.Add("WarLaser", new Component("WarLaser"));
            Components.Add("Drill", new Component("Drill"));
        }
        private static void createFrames()
        {
            if (Frames != null)
            {
                Frames.Clear();
            }
            Frames = new Dictionary<string, Frame>();
            Frames.Add("LightUnit", new Frame("LightUnit"));
            Frames.Add("HeavyUnit", new Frame("HeavyUnit"));
        }
    }
}
