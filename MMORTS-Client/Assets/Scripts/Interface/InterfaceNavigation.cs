using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceNavigation : MonoBehaviour
{
    [SerializeField]
    GameObject Menu;
    
    public void ToMenu()
    {
        Menu.SetActive(true);
    }
    public void CloseMenu()
    {
        Menu.SetActive(false);
    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
