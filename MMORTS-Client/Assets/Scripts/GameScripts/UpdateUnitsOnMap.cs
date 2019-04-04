using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLogic;

public class UpdateUnitsOnMap : MonoBehaviour
{
    [SerializeField]
    GameObject square;
    [SerializeField]
    GameObject cirle;

    Dictionary<int,GameObject> units = new Dictionary<int, GameObject>();
    public void UpdateUnits()
    {
        if (GameManager.CurrentWorld.Units != null)
        {
            for (int i = 0; i < GameManager.CurrentWorld.Units.Count; i++)
            {
                Unit currentUnit = GameManager.CurrentWorld.Units[i];
                if (!units.ContainsKey(currentUnit.uID))
                {
                    GameObject type;
                    switch (currentUnit.UnitType)
                    {
                        case UnitTypes.Circle:
                            type = cirle;
                            break;
                        default:
                            type = square;
                            break;
                    }
                    units.Add(currentUnit.uID, Instantiate(type, currentUnit.UnitPosition, Quaternion.identity));
                    units[currentUnit.uID].GetComponent<UnitInfo>().ID = currentUnit.uID;
                }
            }
        }
    }
}
