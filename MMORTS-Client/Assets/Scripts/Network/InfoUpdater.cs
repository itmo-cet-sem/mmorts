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
        if (time > 0.2f)
        {
            time = 0;
            StartCoroutine("updateInfo");
            Camera.main.GetComponent<UpdateUnitsOnMap>().UpdateUnits();
        }
    }
    IEnumerator updateInfo()
    {
        MessageSender.SendMapMessage();
        yield return new WaitForSeconds(1f);
    }
}
