using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLogic;

public class UpdateUnitsOnMap : MonoBehaviour
{
    [SerializeField]
    GameObject unitTemplate;

    Dictionary<int,GameObject> units = new Dictionary<int, GameObject>();
    public void UpdateUnits()
    {
        if (GameManager.CurrentWorld.Sectors != null)
        {
            foreach (Vector2Int key in GameManager.CurrentWorld.Sectors.Keys)
            {
                if (GameManager.CurrentWorld.Sectors[key].IsActive)
                {
                    if (GameManager.CurrentWorld.Sectors[key].Units != null)
                    {
                        Dictionary<int, Unit> sectorUnits = GameManager.CurrentWorld.Sectors[key].Units;
                        List<int> sectorKeys = new List<int>(sectorUnits.Keys);
                        for (int i=0;i<sectorKeys.Count;i++)
                        {
                            Unit currentUnit = sectorUnits[sectorKeys[i]];
                            if (!units.ContainsKey(currentUnit.uID))
                            {
                                GameObject unit = Instantiate(unitTemplate, currentUnit.UnitPosition, Quaternion.identity);
                                //unit.GetComponent<SpriteRenderer>().sprite = GameManager.UnitTypes[currentUnit.UnitType].Image;
                                units.Add(currentUnit.uID, unit);
                                units[currentUnit.uID].GetComponent<UnitInfo>().ID = currentUnit.uID;

                            }
                            else
                            {
                                units[currentUnit.uID].transform.position = currentUnit.UnitPosition;
                            }
                            units[currentUnit.uID].GetComponent<UnitInfo>().Coordinates = currentUnit.SectorID;
                        }
                    }
                }
            }
        }
    }


}
