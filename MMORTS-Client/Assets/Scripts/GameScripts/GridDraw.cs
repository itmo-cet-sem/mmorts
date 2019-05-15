using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridDraw : MonoBehaviour
{
    public enum GridTypes { vertical, horizontal}
    private static Tilemap TilemapGrid;

    void Awake()
    {
        TilemapGrid = transform.GetChild(0).GetComponent<Tilemap>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer("Grid");
        }
    }
    public static void CreateGridTexture(Vector3Int positon, GridTypes gridType)
    {
        Tile tile;
        switch (gridType)
        {
            case GridTypes.horizontal:
                tile = Resources.Load(@"Textures\Grid\VerticalLine") as Tile;
                break;
            default:
                tile = Resources.Load(@"Textures\Grid\HorizontalLine") as Tile;
                break;
        }
        TilemapGrid.SetTile(positon, tile);
    }
}
