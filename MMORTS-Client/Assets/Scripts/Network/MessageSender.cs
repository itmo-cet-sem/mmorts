﻿using System.Collections;
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
    
    public static void SendMapMessage()
    {
        Connector.SendMessage(MessageBuilder.MapMessage());
    }

    public static void SendSpawnMessage(string unitType)
    {
        Connector.SendMessage(MessageBuilder.SpawnMessage(unitType));
    }

    public static void SendMoveMessage(int uid)
    {
        Connector.SendMessage(MessageBuilder.MoveMessage(uid, GameManager.CurrentWorld.Units[uid].Destination));
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
            GameManager.CurrentWorld.Units.Clear();
            for (int i=0;i< mapInfo.UnitsAttributes.Count;i++)
            {
                Unit tempUnit = new Unit();
                if (mapInfo.UnitsAttributes[i].ContainsKey("ID"))
                {
                    tempUnit.uID = (int)mapInfo.UnitsAttributes[i]["ID"];
                }
                if (mapInfo.UnitsAttributes[i].ContainsKey("Owner"))
                {
                    tempUnit.Owner = GameManager.Players[(int)mapInfo.UnitsAttributes[i]["Owner"]];
                }
                if (mapInfo.UnitsAttributes[i].ContainsKey("type"))
                {
                    tempUnit.UnitType = mapInfo.UnitsAttributes[i]["type"].ToString();
                }
                if (mapInfo.UnitsAttributes[i].ContainsKey("position"))
                {
                    tempUnit.UnitPosition = (Vector3)mapInfo.UnitsAttributes[i]["position"];
                }
                if (mapInfo.UnitsAttributes[i].ContainsKey("destination"))
                {
                    tempUnit.Destination = (Vector3)mapInfo.UnitsAttributes[i]["destination"];
                }
                else
                {
                    tempUnit.Destination = Vector3.negativeInfinity;
                }
                GameManager.CurrentWorld.Units.Add(tempUnit.uID, tempUnit);
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
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] != GameManager.CurrentPlayer.Name)
            {
                GameManager.Players.Add(new Player(names[i]));
            }
            else
            {
                GameManager.Players.Add(GameManager.CurrentPlayer);
            }

        }
    }
    
}

