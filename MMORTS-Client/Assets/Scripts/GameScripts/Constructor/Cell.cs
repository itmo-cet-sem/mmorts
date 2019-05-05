using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameLogic.ComponentPositions ComponentPosition;
    public GameLogic.Component CurrentComponent;
    public void SelectCell()
    {
        transform.parent.GetComponent<ComponentSelector>().CurrentCell = this;
        transform.parent.GetComponent<ComponentSelector>().InitializeWindow();
    }
    public void ExamineCell()
    {
        transform.parent.GetComponent<ComponentSelector>().CurrentCell = this;
        transform.parent.GetComponent<ComponentSelector>().ShowDiscription();
    }
}
