using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLogic;

public class CreateUnitType : MonoBehaviour
{
    [SerializeField]
    InputField Name;

    [SerializeField]
    GameObject CellsField;

    [SerializeField]
    InterfaceNavigation interfaceNavigation;

    public Frame TypeFrame;

    private bool IsView;

    public void ScreenPrepare(bool isView)
    {
        IsView = isView;
        Name.text = "";
        Name.gameObject.SetActive(!IsView);
    }

    public void CreateType()
    {
        if (!IsView)
        {
            if (Name.text != "")
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
        else
        {
            interfaceNavigation.CloseConstructor();
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
        interfaceNavigation.CloseConstructor();
    }
}
