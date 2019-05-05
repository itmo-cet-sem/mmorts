using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellInitializer : MonoBehaviour
{
    [SerializeField]
    GameObject CellsField;

    [SerializeField]
    GameObject CellTemplate;

    [SerializeField]
    ComponentSelector ComponentSelector;

    private List<GameObject> cells;

    public void createCells(GameLogic.Frame frame)
    {
        ComponentSelector.ClearComponentList();
        clearCells();
        placeComponentCell(frame.ArmorComponents, -2, GameLogic.ComponentPositions.Armor);
        placeComponentCell(frame.ToolsComponents, -1, GameLogic.ComponentPositions.Tool);
        placeComponentCell(frame.CoreComponents, 0, GameLogic.ComponentPositions.Core);
        placeComponentCell(frame.MovementComponents, 1, GameLogic.ComponentPositions.Movement);
    }

    public void createCells(GameLogic.UnitType unitType)
    {
        createCells(unitType.UnitFrame);
        placeComponents(unitType);
    }

    private void clearCells()
    {
        if (cells != null)
        {
            foreach (GameObject cell in cells)
            {
                Destroy(cell);
            }
            cells.Clear();
        }
        else
        {
            cells = new List<GameObject>();
        }
    }

    private void placeComponentCell(int count, int row, GameLogic.ComponentPositions position, bool isView = false)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject cell = Instantiate(CellTemplate, CellsField.transform);
            cell.GetComponent<RectTransform>().anchoredPosition =placeCell(cell.GetComponent<RectTransform>(),i, row);
            cell.GetComponent<Cell>().ComponentPosition = position;
            cell.GetComponent<Button>().onClick.AddListener(delegate { cell.GetComponent<Cell>().SelectCell(); });
            cells.Add(cell);
        }
    }

    private void placeComponents(GameLogic.UnitType unitType)
    {
        bool[] isComponentUsed = new bool[unitType.Components.Count];
        for (int i =0;i<cells.Count;i++)
        {
            for (int j=0;j<unitType.Components.Count;j++)
            {
                if (!isComponentUsed[j])
                {
                    if (unitType.Components[j].ComponentPosition == cells[i].GetComponent<Cell>().ComponentPosition)
                    {
                        isComponentUsed[j] = true;
                        Cell cell = cells[i].GetComponent<Cell>();
                        cell.CurrentComponent = unitType.Components[j];
                        cells[i].transform.GetChild(0).GetComponent<Image>().sprite = unitType.Components[j].Image;
                        cells[i].GetComponent<Button>().onClick.RemoveAllListeners();
                        cells[i].GetComponent<Button>().onClick.AddListener(delegate { cell.ExamineCell(); });
                        break;
                    }
                }
            }
        }
    }

    private Vector2 placeCell(RectTransform rectTransform, int i, int j)
    {
        Vector2 position = rectTransform.anchoredPosition;
        position.x -= i * 74 - 90;
        position.y -= j * 74 + 30;
        return position;
    }
}
