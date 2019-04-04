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
    
    public static void SendMapMessage()
    {
        Connector.SendMessage(MessageBuilder.MapMessage());
    }

    public static void SendSpawnMessage(UnitTypes unitType)
    {
        Connector.SendMessage(MessageBuilder.SpawnMessage(unitType));
    }

    public static void SendMoveMessage(int uid)
    {
        Connector.SendMessage(MessageBuilder.MoveMessage(uid, GameManager.CurrentWorld.Units[uid].Destanation));
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
        if (GameManager.LastState != unitsDict.GetHashCode())
        {
            GameManager.LastState = unitsDict.GetHashCode();
            Dictionary<string, object> rawInfo = new Dictionary<string, object>();
            try
            {
                rawInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(unitsDict.ToString());
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                return;
            }

            List<string> playerNames = new List<string>();
            try
            {
                playerNames = JsonConvert.DeserializeObject<List<string>>(rawInfo["player_names"].ToString());
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                return;
            }

            if (playerNames.Count != GameManager.Players.Count)
            {
                createPlayers(playerNames.ToArray());
            }

            List<object> players = new List<object>();
            try
            {
                players = JsonConvert.DeserializeObject<List<object>>(rawInfo["players"].ToString());
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                return;
            }

            GameManager.CurrentWorld.Units = new Dictionary<int, Unit>();

            for (int i = 0; i < players.Count; i++)
            {
                Dictionary<string, object> units = new Dictionary<string, object>();
                try
                {
                    units = JsonConvert.DeserializeObject<Dictionary<string, object>>(players[i].ToString());
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    return;
                }
                for (int j = 0; j < units.Count; j++)
                {
                    Dictionary<string, object> unitsProperties = new Dictionary<string, object>();
                    try
                    {
                        unitsProperties = JsonConvert.DeserializeObject<Dictionary<string, object>>(units[j.ToString()].ToString());
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        return;
                    }
                    Unit tempUnit = new Unit();
                    tempUnit.Owner = GameManager.Players[i];
                    if (unitsProperties.ContainsKey("uid"))
                    {
                        int id = 0;
                        try
                        {
                            id = int.Parse(unitsProperties["uid"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            return;
                        }
                        tempUnit.uID = id;
                    }

                    if (unitsProperties.ContainsKey("type"))
                    {
                        switch (unitsProperties["type"])
                        {
                            case "basic_circle":
                                tempUnit.UnitType = UnitTypes.Circle;
                                break;
                            default:
                                tempUnit.UnitType = UnitTypes.Square;
                                break;
                        }
                    }

                    if (unitsProperties.ContainsKey("position"))
                    {
                        List<float> coorditates = new List<float>();
                        try
                        {
                            coorditates = JsonConvert.DeserializeObject<List<float>>(unitsProperties["position"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            return;
                        }
                        tempUnit.UnitPosition = new Vector3(coorditates[1], coorditates[2], coorditates[0]);
                    }
                    if (unitsProperties.ContainsKey("destination"))
                    {
                        List<float> coorditates = new List<float>();
                        try
                        {
                            coorditates = JsonConvert.DeserializeObject<List<float>>(unitsProperties["destination"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            return;
                        }
                        tempUnit.Destanation = new Vector3(coorditates[1], coorditates[2], coorditates[0]);
                    }
                    GameManager.CurrentWorld.Units.Add(tempUnit.uID, tempUnit);
                }
            }
        }
    }
    private static void createPlayers(string[] names)
    {
        GameManager.Players.Clear();
        for (int i = 0; i < names.Length; i++)
        {
            GameManager.Players.Add(new Player(names[i]));
        }
    }
    
}

