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
        if (GameManager.CurrentWorld.Units != null)
        {
            for (int i = 0; i < GameManager.CurrentWorld.Units.Count; i++)
            {
                Unit currentUnit = GameManager.CurrentWorld.Units[i];
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
            }
        }
    }


}
