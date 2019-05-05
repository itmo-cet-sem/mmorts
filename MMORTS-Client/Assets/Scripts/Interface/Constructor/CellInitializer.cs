using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void placeComponentCell(int count, int row, GameLogic.ComponentPositions position)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject cell = Instantiate(CellTemplate, CellsField.transform);
            cell.transform.position =placeCell(cell.transform.position,i, row);
            cell.GetComponent<Cell>().ComponentPosition = position;
            cells.Add(cell);
        }
    }

    private Vector3 placeCell(Vector3 position, int i, int j)
    {
        position.x -= i * 120 - 90;
        position.y -= j * 120 + 30;
        return position;
    }
}
