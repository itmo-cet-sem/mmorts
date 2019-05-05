using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    public void Close()
    {
        Destroy(gameObject);
    }
}
