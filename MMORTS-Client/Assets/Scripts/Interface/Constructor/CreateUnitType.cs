using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLogic;

public class CreateUnitType : MonoBehaviour
{
    [SerializeField]
    Text Name;

    [SerializeField]
    GameObject CellsField;

    public Frame TypeFrame;

    public void CreateType()
    {
        if (Name.text!="")
        {
            if (!GameManager.UnitTypes.ContainsKey(Name.text))
            {
                createType();
            }
            else
            {
                CreateMessage.CreateWarningMessage("Error", "This name is already taken", transform);
            }
        }
        else
        {
            CreateMessage.CreateWarningMessage("Error", "Enter unit type name", transform);
        }
    }

    private void createType()
    {
        List<GameLogic.Component> components = new List<GameLogic.Component>();
        for (int i=0;i<CellsField.transform.childCount;i++)
        {
            GameLogic.Component cellComponent = CellsField.transform.GetChild(i).GetComponent<Cell>().CurrentComponent;
            components.Add(cellComponent);
        }
        UnitType newType = new UnitType(TypeFrame, components, Name.text);
        MessageSender.SendRegisterUnitTypeMessage(newType);
        MessageSender.SendGetUnitTypesMessage();
    }
}
