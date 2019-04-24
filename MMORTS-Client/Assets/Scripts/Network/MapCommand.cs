using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCommand : Command
{
    // Command structure
    //      Dictionary of categories                1
    //      |                      |
    //      V                      V
    //List of players     List of players names     2
    //        |
    //        V
    //List of units                                 3
    //      |
    //      V
    //Dictionary of attributes                      4

    private List<string> _playerNames;
    public List<string> PlayerNames
    {
        get
        {
            return _playerNames;
        }
    }

    public List<Dictionary<string, object>> UnitsAttributes { get; }

    public MapCommand()
    {
        _playerNames = new List<string>();
        UnitsAttributes = new List<Dictionary<string, object>>();
    }

    public bool ProccessCommand(object CommandInfo)
    {
        // First layer
        Dictionary<string, object> rawInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(CommandInfo.ToString());
        if (rawInfo == null)
        {
            return false;
        }

        // Second layer
        _playerNames = JsonConvert.DeserializeObject<List<string>>(rawInfo["player_names"].ToString());
        if (PlayerNames == null)
        {
            return false;
        }

        List<object> players = JsonConvert.DeserializeObject<List<object>>(rawInfo["players"].ToString()); ;
        if (players == null)
        {
            return false;
        }

        //Third layer
        for (int i = 0; i < players.Count; i++)
        {
            Dictionary<string, object> units = JsonConvert.DeserializeObject<Dictionary<string, object>>(players[i].ToString());
            if (units == null)
            {
                return false;
            }
            for (int j = 0; j < units.Count; j++)
            {
                //Fourth layer
                Dictionary<string, object> unitsProperties = JsonConvert.DeserializeObject<Dictionary<string, object>>(units[j.ToString()].ToString());
                if (unitsProperties == null)
                {
                    return false;
                }
                UnitsAttributes.Add(new Dictionary<string, object>());
                UnitsAttributes[UnitsAttributes.Count - 1].Add("Owner", i);

                if (unitsProperties.ContainsKey("uid"))
                {
                    int id = int.Parse(unitsProperties["uid"].ToString());
                    UnitsAttributes[UnitsAttributes.Count - 1].Add("ID", id);
                }

                if (unitsProperties.ContainsKey("type"))
                {
                    UnitsAttributes[UnitsAttributes.Count - 1].Add("type", unitsProperties["type"]);
                }

                if (unitsProperties.ContainsKey("position"))
                {
                    List<float> coorditates = JsonConvert.DeserializeObject<List<float>>(unitsProperties["position"].ToString());
                    if (coorditates == null)
                    {
                        return false;
                    }

                    UnitsAttributes[UnitsAttributes.Count - 1].Add("position", new Vector3(coorditates[1], coorditates[2], coorditates[0]));
                }
                if (unitsProperties.ContainsKey("destination"))
                {
                    List<float> coorditates = JsonConvert.DeserializeObject<List<float>>(unitsProperties["destination"].ToString());
                    if (coorditates == null)
                    {
                        return false;
                    }
                    UnitsAttributes[UnitsAttributes.Count - 1].Add("destination", new Vector3(coorditates[1], coorditates[2], coorditates[0]));
                }
            }
        }
        return true;
    }
}
