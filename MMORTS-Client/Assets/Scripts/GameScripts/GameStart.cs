using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [SerializeField]
    Text ConnectionString;
    float time = 0;
    float attemptTime = 30;
    // Start is called before the first frame update
    void Start()
    {
        connectAttempt();
        GameLogic.GameManager.onStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Connector.IsConnected)
        {
            if (time< attemptTime)
            {
                ConnectionString.text = "Connecting... " + ((int)(attemptTime - time)).ToString();
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                connectAttempt();
            }
        }
    }
    public void connectAttempt()
    {
        Connector.ConnectToServer();
        if (Connector.IsConnected)
        {
            ConnectionString.text = "Connected";
        }
        else
        {
            ConnectionString.text = "Connecting...";
        }
    }
}
