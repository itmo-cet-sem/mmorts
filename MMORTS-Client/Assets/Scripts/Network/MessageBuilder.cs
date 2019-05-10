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
    public static string MapMessage(int space, List<Vector2Int> sectors)
    {
        Dictionary<string, object> command = new Dictionary<string, object>();
        command.Add("c", "map");
        command.Add("space", space);
        List<int[]> sectorsXY = new List<int[]>();
        for (int i = 0; i<sectors.Count;i++)
        {
            sectorsXY.Add(new int[2]);
            sectorsXY[i][0] = sectors[i].x;
            sectorsXY[i][1] = sectors[i].y;
        }
        command.Add("sectors", sectorsXY);
        return JsonConvert.SerializeObject(command);
    }
    public static string SpawnMessage(string unitType)
    {
        Dictionary<string, string> command = new Dictionary<string, string>();
        command.Add("c", "spawn_unit");
        command.Add("unit_type", unitType);
        return JsonConvert.SerializeObject(command);
    }
    public static string MoveMessage(int uid, int space, Vector2Int sector, Vector3 destonation)
    {
        Dictionary<string, object> command = new Dictionary<string, object>();
        command.Add("c", "move_unit");
        command.Add("uid", uid);
        List<float> coords = new List<float>();
        coords.Add(space);
        coords.Add(sector.x);
        coords.Add(sector.y);
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
            if (unitType.Components[i] != null)
            {
                components.Add(unitType.Components[i].Name);
            }
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