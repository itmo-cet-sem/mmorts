using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLogic;

public class UnitTypeSelect : ScrollSelector
{
    [SerializeField]
    GameObject DeleteButton;
    [SerializeField]
    GameObject EditButton;
    [SerializeField]
    Text DesriptionText;

    private int i;

    override protected void createButtons()
    {
        DeleteButton.SetActive(false);
        EditButton.SetActive(false);
        DesriptionText.text = "";
        i = 0;
        foreach (string key in GameManager.UnitTypes.Keys)
        {
            i++;
            createUnitTypeButton(GameManager.UnitTypes[key]);
        }
    }

    private void createUnitTypeButton(UnitType unitType)
    {
        GameObject button = Instantiate(ButtonTemplate, Content.transform);
        button.transform.GetChild(0).GetComponent<Text>().text = unitType.Name;
        button.GetComponent<Button>().onClick.AddListener(delegate { setSelectedUnit(unitType.Name); });
        addButtonToList(button, i);
    }
    private void setSelectedUnit(string name)
    {
        DesriptionText.text = name;
        DesriptionText.text += "\nFrame: " + GameManager.UnitTypes[name].UnitFrame.Name;
        DesriptionText.text += "\nSize: " + GameManager.UnitTypes[name].Size.ToString();
        DesriptionText.text += "\nComponents used: " + GameManager.UnitTypes[name].Components.Count.ToString();
        GameManager.CurrentWorld.TempSelectedType = GameManager.UnitTypes[name];
        //DeleteButton.SetActive(true);
        //EditButton.SetActive(true);
    }
}
