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
    public static string SpawnMessage(string unitType)
    {
        Dictionary<string, string> command = new Dictionary<string, string>();
        command.Add("c", "spawn_unit");
        command.Add("unit_type", unitType);
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
    public static string RegisterUnitTypeMessage(GameLogic.UnitType unitType)
    {
        Dictionary<string, object> command = new Dictionary<string, object>();
        command.Add("c", "register_unit_type");
        command.Add("unit_type", unitType.Name);
        Dictionary<string, object> commandParams = new Dictionary<string, object>();
        List<string> components = new List<string>();
        for (int i=0;i<unitType.Components.Count;i++)
        {
            components.Add(unitType.Components[i].Name);
        }
        commandParams.Add("components", components);
        commandParams.Add("frame", unitType.UnitFrame.Name);
        command.Add("params", commandParams);
        return JsonConvert.SerializeObject(command);
    }

    public static string GetUnitTypesMessage()
    {
        Dictionary<string, object> command = new Dictionary<string, object>();
        command.Add("c", "get_unit_types");
        return JsonConvert.SerializeObject(command);
    }
}