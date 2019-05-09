using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GameLogic;
using System;
using System.Linq;

public static class MessageSender
{
    public static bool goToGameplayScene = false;
    public static void SendLoginMessage(string login)
    {
        Connector.SendMessage(MessageBuilder.LoginMessage(login));
        Connector.OnAnswerRecieve += loginAnswer;
    }
    
    public static void SendMapMessage(int space, Dictionary<Vector2Int, Sector> sectors)
    {
        List<Vector2Int> sectorsList = new List<Vector2Int>();
        foreach (Vector2Int key in sectors.Keys)
        {
            if (sectors[key].IsActive)
            {
                sectorsList.Add(key);
            }
        }
        Connector.SendMessage(MessageBuilder.MapMessage(space, sectorsList));
    }

    public static void SendSpawnMessage(string unitType)
    {
        Connector.SendMessage(MessageBuilder.SpawnMessage(unitType));
    }

    public static void SendMoveMessage(int uid, int space, Sector sector)
    {
        Vector2Int coordinates = new Vector2Int((int)sector.Units[uid].Destination.x / Config.SectorSize, (int)sector.Units[uid].Destination.z / Config.SectorSize);
        Vector3 destination = new Vector3(sector.Units[uid].Destination.x - coordinates.x * 10, sector.Units[uid].Destination.y - coordinates.y * 10);
        Connector.SendMessage(MessageBuilder.MoveMessage(uid, space, coordinates, destination));
    }

    public static void SendRegisterUnitTypeMessage(UnitType unitType)
    {
        Connector.SendMessage(MessageBuilder.RegisterUnitTypeMessage(unitType));
    }

    public static void SendGetUnitTypesMessage()
    {
        Connector.SendMessage(MessageBuilder.GetUnitTypesMessage());
        Connector.OnUnitsTypesUpdate += updateUnitsTypes;
    }

    public static void StartingGetUnitInfo()
    {
        Connector.OnUnitsUpdate += updateUnits;
    }

    private static void loginAnswer(string answer)
    {
        Connector.OnAnswerRecieve -= loginAnswer;
        if (answer == GameManager.CurrentPlayer.Name)
        {
            goToGameplayScene = true; // not load scene from here, so i did that strange thing with bool
        }
        else
        {
            Debug.Log("Wrong login");
        }
    }


    private static void updateUnits(object unitsDict)
    {
        MapCommand mapInfo = new MapCommand();
        bool isSuccess = mapInfo.ProccessCommand(unitsDict);

        if (isSuccess)
        {
            if (mapInfo.PlayerNames.Count != GameManager.Players.Count)
            {
                createPlayers(mapInfo.PlayerNames.ToArray());
            }
            foreach (Vector2Int key in mapInfo.UnitsAttributes.Keys)
            {
                GameManager.CurrentWorld.Sectors[key].Units.Clear();
                for (int i = 0; i < mapInfo.UnitsAttributes[key].Count; i++)
                {
                    Unit tempUnit = new Unit();
                    tempUnit.SectorID = key;
                    if (mapInfo.UnitsAttributes[key][i].ContainsKey("ID"))
                    {
                        tempUnit.uID = (int)mapInfo.UnitsAttributes[key][i]["ID"];
                    }
                    if (mapInfo.UnitsAttributes[key][i].ContainsKey("player"))
                    {
                        int playerID = GameManager.GetPlayerIDByName(mapInfo.UnitsAttributes[key][i]["player"].ToString());
                        tempUnit.Owner = GameManager.Players[playerID];
                    }
                    if (mapInfo.UnitsAttributes[key][i].ContainsKey("type"))
                    {
                        tempUnit.UnitType = mapInfo.UnitsAttributes[key][i]["type"].ToString();
                    }
                    if (mapInfo.UnitsAttributes[key][i].ContainsKey("position"))
                    {
                        tempUnit.UnitPosition = (Vector3)mapInfo.UnitsAttributes[key][i]["position"];
                    }
                    if (mapInfo.UnitsAttributes[key][i].ContainsKey("destination"))
                    {
                        tempUnit.Destination = (Vector3)mapInfo.UnitsAttributes[key][i]["destination"];
                    }
                    else
                    {
                        tempUnit.Destination = Vector3.negativeInfinity;
                    }
                    GameManager.CurrentWorld.Sectors[key].Units.Add(tempUnit.uID, tempUnit);
                }  
            }
        }

    }

    private static void updateUnitsTypes(object types)
    {
        Connector.OnUnitsTypesUpdate -= updateUnitsTypes;
        UnitTypesCommand typesCommand = new UnitTypesCommand();
        bool isSuccess = typesCommand.ProccessCommand(types);
        if (isSuccess)
        {
            GameManager.UnitTypes = typesCommand.unitsTypes;
        }
    }

    private static void createPlayers(string[] names)
    {
        GameManager.Players.Clear();
        GameManager.Players.Add(GameManager.CurrentPlayer);
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] != GameManager.CurrentPlayer.Name)
            {
                GameManager.Players.Add(new Player(names[i]));
            }

        }
    }
    
}

