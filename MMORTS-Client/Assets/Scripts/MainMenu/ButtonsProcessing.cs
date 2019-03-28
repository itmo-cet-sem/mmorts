using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLogic;

public class ButtonsProcessing : MonoBehaviour
{
    [SerializeField]
    InputField messageBox;

    public void ExitGame()
    {
        if (Connector.IsConnected)
        {
            Connector.CloseConnection();
        }
        Application.Quit();
    }

    public void ConnectToServer()
    {
        Connector.ConnectToServer();
    }

    public void SendMessage()
    {
        if (Connector.IsConnected)
        {
            Connector.SendMessage(messageBox.text);
        }
        else
        {
            Debug.Log("no connection");
        }
    }
}
