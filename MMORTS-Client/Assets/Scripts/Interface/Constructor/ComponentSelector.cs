using GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentSelector : ScrollSelector
{
    [SerializeField]
    Text DesriptionText;

    public Cell CurrentCell;

    private int i;

    public void ClearComponentList()
    {
        clearButtons();
        DesriptionText.text = "";
    }

    override protected void createButtons()
    {
        i = 0;
        foreach (string key in GameManager.Components.Keys)
        {
            if (GameManager.Components[key].ComponentPosition == CurrentCell.ComponentPosition)
            {
                i++;
                createComponentButton(GameManager.Components[key], i);
            }
        }
    }

    private void createComponentButton(GameLogic.Component component, int i)
    {
        GameObject button = Instantiate(ButtonTemplate, Content.transform);
        button.transform.GetChild(0).GetComponent<Text>().text = component.Name;
        button.GetComponent<Button>().onClick.AddListener(delegate { setSelectedComponent(component.Name); });
        addButtonToList(button, i);
    }
    private void setSelectedComponent(string name)
    {
        DesriptionText.text = name;
        DesriptionText.text += "\nComponent Weight: " + GameManager.Components[name].Weight;
        CurrentCell.CurrentComponent = GameManager.Components[name];
        CurrentCell.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Components[name].Image;
        //EditButton.SetActive(true);
    }
}
