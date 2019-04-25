using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoUpdater : MonoBehaviour
{
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("updateInfo");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (Connector.IsConnected)
        {
            if (time > 0.02f)
            {
                time = 0;
                StartCoroutine("updateInfo");
                Camera.main.GetComponent<UpdateUnitsOnMap>().UpdateUnits();
            }
        }
        else
        {
            if (time > 30f)
            {
                Connector.ConnectToServer();
                MessageSender.SendLoginMessage(GameLogic.GameManager.CurrentPlayer.Name);
            }
        }
    }
    IEnumerator updateInfo()
    {
        MessageSender.SendMapMessage();
        yield return new WaitForSeconds(1f);
    }
}
