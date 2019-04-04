using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class MessageBuilder
{
    public static string LoginMessage(string login)
    {
        Dictionary<string, string> command = new Dictionary<string, string>();
        command.Add("c","login");
        command.Add("player_name", login);
        return JsonConvert.SerializeObject(command);
    }
    public static string MapMessage()
    {
        Dictionary<string, string> command = new Dictionary<string, string>();
        command.Add("c", "map");
        return JsonConvert.SerializeObject(command);
    }
    public static string SpawnMessage(GameLogic.UnitTypes unitType)
    {
        Dictionary<string, string> command = new Dictionary<string, string>();
        command.Add("c", "spawn_unit");
        string unitTypeString;
        switch (unitType)
        {
            case GameLogic.UnitTypes.Circle:
                unitTypeString = "basic_circle";
                break;
            default:
                unitTypeString = "basic_square";
                break;
        }
        command.Add("unit_type", unitTypeString);
        return JsonConvert.SerializeObject(command);
    }
    public static string MoveMessage(int uid, Vector3 destonation)
    {
        Dictionary<string, object> command = new Dictionary<string, object>();
        command.Add("c", "move_unit");
        command.Add("uid", uid);
        List<float> coords = new List<float>();
        coords.Add(0);
        coords.Add(destonation.x);
        coords.Add(destonation.y);
        command.Add("destination", coords);
        return JsonConvert.SerializeObject(command);
    }
}