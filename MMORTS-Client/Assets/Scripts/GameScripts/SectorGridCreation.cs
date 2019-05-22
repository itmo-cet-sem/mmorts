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
        for (int i = 0; i < Config.SectorSize; i++)
        {
            for (int j = 0; j < Config.SectorSize; j++)
            {
                int x = positon.x* Config.SectorSize + i;
                int y = positon.y* Config.SectorSize + j;
                Vector3Int tilePositon = new Vector3Int(x, y, 0);
                TilemapGrid.SetTile(tilePositon, selectTile(x,y));
                checkBorder(tilePositon,i, j);
            }
        }
    }

    private static void checkBorder(Vector3Int position,int i, int j)
    {
        if (i==0)
        {
            if (j==0)
            {
                GridDraw.CreateGridTexture(position, GridDraw.GridTypes.horizontal);
                GridDraw.CreateGridTexture(position, GridDraw.GridTypes.vertical);
            }
            else if (j==Config.SectorSize)
            {
                GridDraw.CreateGridTexture(position, GridDraw.GridTypes.horizontal);
                GridDraw.CreateGridTexture(position, GridDraw.GridTypes.vertical);
            }
            else
            {
                GridDraw.CreateGridTexture(position, GridDraw.GridTypes.horizontal);
            }
        }
        else if (i == Config.SectorSize)
        {
            if (j == Config.SectorSize)
            {
                GridDraw.CreateGridTexture(position, GridDraw.GridTypes.horizontal);
                GridDraw.CreateGridTexture(position, GridDraw.GridTypes.vertical);
            }
            else if (i==0)
            {
                GridDraw.CreateGridTexture(position, GridDraw.GridTypes.horizontal);
                GridDraw.CreateGridTexture(position, GridDraw.GridTypes.vertical);
            }
            else
            {
                GridDraw.CreateGridTexture(position, GridDraw.GridTypes.horizontal);
            }
        }
        if (j == Config.SectorSize || j == 0)
        {
            GridDraw.CreateGridTexture(position, GridDraw.GridTypes.vertical);
        }
    }

    private static Tile selectTile(int x, int y)
    {
        float scale = 10000;
        float noise = Mathf.PerlinNoise((x+Config.RandomTile.Next(-10000,10000))/ scale, (y + Config.RandomTile.Next(-10000, 10000))/ scale);
        int tileNumber = Mathf.RoundToInt(noise);
        Tile tile;
        switch (tileNumber)
        {
            case 1:
                tile = Resources.Load(@"Textures\Tiles\RockTile") as Tile;
                break;
            default:
                tile = Resources.Load(@"Textures\Tiles\DirtTile") as Tile;
                break;
        }
        return tile;
    }
}
