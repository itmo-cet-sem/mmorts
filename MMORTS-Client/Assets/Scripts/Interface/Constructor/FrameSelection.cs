using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLogic;

public class FrameSelection : ScrollSelector
{
    [SerializeField]
    GameObject CreateButton;
    [SerializeField]
    Text DesriptionText;

    public string SelectedFrame;

    private int i;

    override protected void createButtons()
    {
        CreateButton.SetActive(false);
        DesriptionText.text = "";
        SelectedFrame = "";
        i = 0;
        foreach (string key in GameManager.Frames.Keys)
        {
            i++;
            createFrameButton(GameManager.Frames[key]);
        }
    }

    private void createFrameButton(Frame frame)
    {
        GameObject button = Instantiate(ButtonTemplate, Content.transform);
        button.transform.GetChild(0).GetComponent<Text>().text = frame.GameName;
        button.GetComponent<Button>().onClick.AddListener(delegate { setSelectedFrame(frame.Name); });
        addButtonToList(button, i);
    }
    private void setSelectedFrame(string name)
    {
        DesriptionText.text = GameManager.Frames[name].GameName;
        DesriptionText.text += "\nComponent Count: " + GameManager.Frames[name].ComponentsAvalible.ToString();
        DesriptionText.text += "\nSize: " + GameManager.Frames[name].Size.ToString();
        SelectedFrame = name;
        CreateButton.SetActive(true);
        //EditButton.SetActive(true);
    }
}
