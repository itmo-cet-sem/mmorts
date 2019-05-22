using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    public const int FieldOfViewRadius = 5;
    public const int SectorSize = 10;
    public const int MapSeed = 42;
    public static System.Random RandomTile = new System.Random(MapSeed);
    public static string ServerAdress = "127.0.0.1";
    public static int Port = 31337;
}
