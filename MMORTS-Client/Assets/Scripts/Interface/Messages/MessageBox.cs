using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    public void Close()
    {
        if (OnCloseButton != null)
        {
            OnCloseButton();
        }
        Destroy(gameObject);
    }
    public delegate void CloseButton();
    public event CloseButton OnCloseButton;
}
