using GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSelector : MonoBehaviour
{
    [SerializeField]
    protected GameObject Content;
    [SerializeField]
    protected GameObject ButtonTemplate;

    protected List<GameObject> buttons;

    public void InitializeWindow()
    {
        clearButtons();
        createButtons();
    }

    protected virtual void createButtons()
    {

    }

    protected void clearButtons()
    {
        if (buttons != null)
        {
            foreach (GameObject button in buttons)
            {
                Destroy(button);
            }
            buttons.Clear();
        }
        else
        {
            buttons = new List<GameObject>();
        }
    }

    protected void addButtonToList(GameObject button, int i)
    {
        Vector3 pos = button.transform.position;
        pos.y -= i * 60;
        button.transform.position = pos;
        buttons.Add(button);
    }

}
