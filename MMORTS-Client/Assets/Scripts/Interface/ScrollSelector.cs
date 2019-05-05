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
        RectTransform rect = button.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - i * 40);
        buttons.Add(button);
    }

}
