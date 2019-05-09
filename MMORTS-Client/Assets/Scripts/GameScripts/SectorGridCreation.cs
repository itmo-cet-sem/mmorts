using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SectorGridCreation : MonoBehaviour
{
    private static Tilemap TilemapGrid;

    void Awake()
    {
        TilemapGrid = transform.GetChild(0).GetComponent<Tilemap>();
    }
    /*public static GameObject CreateGrid(Vector2Int sectorPosition)
    {
        GameObject gridPrefab = Resources.Load(@"Prefabs\SectorGrid") as GameObject;
        Vector3 position = new Vector3(sectorPosition.x * 10, sectorPosition.y * 10, 0);
        GameObject sector = Instantiate(gridPrefab, position, Quaternion.identity);
        createGridTexture(sector.transform.GetChild(0).GetComponent<Tilemap>());
        return sector;
    }*/

    public static void CreateGridTexture(Vector2Int positon)
    {
        Tile dirtTile = Resources.Load(@"Textures\DirtTile") as Tile;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                int x = positon.x*10 + i;
                int y = positon.y*10 + j;
                Vector3Int tilePositon = new Vector3Int(x, y, 0);
                TilemapGrid.SetTile(tilePositon,dirtTile);
            }
        }
    }
}
