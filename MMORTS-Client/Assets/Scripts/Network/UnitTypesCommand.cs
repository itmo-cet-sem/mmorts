using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLogic;

public class UnitTypesCommand : Command
{
    public Dictionary<string,UnitType> unitsTypes;
    public UnitTypesCommand()
    {
        unitsTypes = new Dictionary<string, UnitType>();
    }

    public bool ProccessCommand(object commandInfo)
    {
        List<object> types = JsonConvert.DeserializeObject<List<object>>(commandInfo.ToString());
        for (int i=0;i<types.Count;i++)
        {
            Dictionary<string,object> type = JsonConvert.DeserializeObject<Dictionary<string, object>>(types[i].ToString());
            string name ="";
            if (type.ContainsKey("name"))
            {
                name = type["name"].ToString();
            }
            List<GameLogic.Component> components = new List<GameLogic.Component>();
            Frame frame = null;
            if (type.ContainsKey("params"))
            {
                Dictionary<string, object> typeParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(type["params"].ToString());
                if (typeParams.ContainsKey("components"))
                {
                    List<string> compontensKeys = JsonConvert.DeserializeObject<List<string>>(typeParams["components"].ToString());
                    for (int j=0;j< compontensKeys.Count;j++)
                    {
                        components.Add(GameManager.Components[compontensKeys[j]]);
                    }
                }
                if (typeParams.ContainsKey("frame"))
                {
                    frame = GameManager.Frames[typeParams["frame"].ToString()];
                }
            }
            if (frame != null)
            {
                UnitType tempType = new UnitType(frame, components, name);
                unitsTypes.Add(name,tempType);
            }
        }

        return true;
    }
}
