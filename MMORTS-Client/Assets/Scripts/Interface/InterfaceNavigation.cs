using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceNavigation : MonoBehaviour
{
    [SerializeField]
    GameObject Menu;

    [SerializeField]
    GameObject ConstructorSelector;

    [SerializeField]
    GameObject Constructor;

    public void ToMenu()
    {
        Menu.SetActive(true);
    }
    public void CloseMenu()
    {
        Menu.SetActive(false);
    }
    public void CloseConstructorSelector()
    {
        ConstructorSelector.SetActive(false);
    }
    public void CloseConstructor()
    {
        Constructor.SetActive(false);
    }
    public void OpenConstructor()
    {
        openConstructor();
        GameLogic.Frame selectedFrame = GameLogic.GameManager.Frames[ConstructorSelector.GetComponent<FrameSelection>().SelectedFrame];
        Constructor.GetComponent<CellInitializer>().createCells(selectedFrame);
        Constructor.GetComponent<CreateUnitType>().TypeFrame = selectedFrame;
        Constructor.GetComponent<CreateUnitType>().ScreenPrepare(false);
    }
    public void OpenConstructorView()
    {
        openConstructor();
        GameLogic.UnitType selectedUnit = GameLogic.GameManager.UnitTypes[ConstructorSelector.GetComponent<UnitTypeSelect>().SelectedUnitType];
        Constructor.GetComponent<CellInitializer>().createCells(selectedUnit);
        Constructor.GetComponent<CreateUnitType>().ScreenPrepare(true);
    }
    public void OpenConstructorSelector()
    {
        ConstructorSelector.SetActive(true);
        ConstructorSelector.GetComponent<UnitTypeSelect>().InitializeWindow();
        ConstructorSelector.GetComponent<FrameSelection>().InitializeWindow();
    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    private void openConstructor()
    {
        ConstructorSelector.SetActive(false);
        Constructor.SetActive(true);
    }
}
