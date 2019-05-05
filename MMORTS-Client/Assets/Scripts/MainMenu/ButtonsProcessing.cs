using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ButtonsProcessing : MonoBehaviour
{
    [SerializeField]
    InputField loginBox;

    public void ExitGame()
    {
        if (Connector.IsConnected)
        {
            Connector.CloseConnection();
        }
        Application.Quit();
    }

    public void Login()
    {
        if (Connector.IsConnected)
        {
            MessageSender.SendLoginMessage(loginBox.text);
            GameLogic.GameManager.CurrentPlayer = new GameLogic.Player(loginBox.text);
        }
        else
        {
            Debug.Log("no connection");
            GetComponent<GameStart>().connectAttempt();
        }
    }

    private void Update()
    {
        if (MessageSender.goToGameplayScene)
        {
            SceneManager.LoadScene(1); // strange thing with bool
            MessageSender.goToGameplayScene= false;
        }
    }
}
