using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCommand : Command
{
    private List<string> _playerNames;
    public List<string> PlayerNames
    {
        get
        {
            return _playerNames;
        }
    }

    public int Space;
    public Dictionary<Vector2Int,List<Dictionary<string, object>>> UnitsAttributes { get; }

    public MapCommand()
    {
        _playerNames = new List<string>();
        UnitsAttributes = new Dictionary<Vector2Int, List<Dictionary<string, object>>>();
    }

    public bool ProccessCommand(object CommandInfo)
    {
        List<object> sectors = JsonConvert.DeserializeObject<List<object>>(CommandInfo.ToString());
        if (sectors == null)
        {
            return false;
        }

        for (int i = 0; i < sectors.Count; i++)
        {
            Dictionary<string, object> sectorInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(sectors[i].ToString());
            if (sectorInfo == null)
            {
                return false;
            }
            int space = 0;
            if (sectorInfo.ContainsKey("space"))
            {
                space = int.Parse(sectorInfo["space"].ToString());
            }

            List<int> rawSectorPosition = null;
            if (sectorInfo.ContainsKey("sector"))
            {
                rawSectorPosition = JsonConvert.DeserializeObject<List<int>>(sectorInfo["sector"].ToString()); ;
            }
            if (rawSectorPosition == null)
            {
                return false;
            }
            Vector2Int sectorPosition = new Vector2Int(rawSectorPosition[0], rawSectorPosition[1]);

            List<object> units = null;
            if (sectorInfo.ContainsKey("units"))
            {
                units = JsonConvert.DeserializeObject<List<object>>(sectorInfo["units"].ToString());
            }
            if (units == null)
            {
                return false;
            }

            UnitsAttributes.Add(sectorPosition, new List<Dictionary<string, object>>());

            for (int j = 0; j < units.Count; j++)
            {
                Dictionary<string, object> unitsProperties = JsonConvert.DeserializeObject<Dictionary<string, object>>(units[j].ToString());
                if (unitsProperties == null)
                {
                    return false;
                }
                UnitsAttributes[sectorPosition].Add(new Dictionary<string, object>());

                UnitsAttributes[sectorPosition][j].Add("Sector", sectorPosition);

                if (unitsProperties.ContainsKey("uid"))
                {
                    int id = int.Parse(unitsProperties["uid"].ToString());
                    UnitsAttributes[sectorPosition][j].Add("ID", id);
                }

                if (unitsProperties.ContainsKey("player"))
                {
                    string player = unitsProperties["player"].ToString();
                    UnitsAttributes[sectorPosition][j].Add("player", player);
                    if (!_playerNames.Contains(player))
                    {
                        _playerNames.Add(player);
                    }
                }

                if (unitsProperties.ContainsKey("type"))
                {
                    UnitsAttributes[sectorPosition][j].Add("type", unitsProperties["type"]);
                }

                if (unitsProperties.ContainsKey("position"))
                {
                    List<float> coordinates = JsonConvert.DeserializeObject<List<float>>(unitsProperties["position"].ToString());
                    if (coordinates == null)
                    {
                        return false;
                    }

                    UnitsAttributes[sectorPosition][j].Add("position", from5to2(coordinates));
                }
                if (unitsProperties.ContainsKey("destination"))
                {
                    List<float> coordinates = JsonConvert.DeserializeObject<List<float>>(unitsProperties["destination"].ToString());
                    if (coordinates == null)
                    {
                        return false;
                    }
                    UnitsAttributes[sectorPosition][j].Add("destination", from5to2(coordinates));
                }
            }
        }
        if (!_playerNames.Contains(GameLogic.GameManager.CurrentPlayer.Name))
        {
            _playerNames.Add(GameLogic.GameManager.CurrentPlayer.Name);
        }
        return true;
    }
    private Vector3 from5to2(List<float> coordinates)
    {
        return new Vector3(coordinates[1] * Config.SectorSize + coordinates[3], coordinates[2] * Config.SectorSize + coordinates[4], 0);
    }
}
