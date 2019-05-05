using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMessage: MonoBehaviour
{
    public static void CreateWarningMessage(string title, string text, Transform parent, MessageBox.CloseButton closeButtonEvent = null)
    {
        GameObject template = Resources.Load(@"Prefabs\System\Messages\Warning") as GameObject;
        GameObject warning = Instantiate(template, parent);
        warning.transform.GetChild(0).GetComponent<Text>().text = title;
        warning.transform.GetChild(1).GetComponent<Text>().text = text;
        warning.GetComponent<MessageBox>().OnCloseButton += closeButtonEvent;
    }
}
