using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LoadGameScene : MonoBehaviour
{
    [SerializeField]
    Tilemap map;
    [SerializeField]
    Tile dirtTile;
    // Start is called before the first frame update
    void Start()
    {
        loadMap();
        MessageSender.StartingGetUnitInfo();
        GameLogic.GameManager.CurrentWorld = new GameLogic.World();
        GameLogic.GameManager.Players = new List<GameLogic.Player>();
        GameLogic.GameManager.Players.Add(GameLogic.GameManager.CurrentPlayer);
        GameLogic.GameManager.LastState = 0;
    }

    void loadMap()
    {
        for (int i = -100; i < 100; i++)
        {
            for (int j = -100; j < 100; j++)
            {
                map.BoxFill(new Vector3Int(i,j,0), dirtTile, i, j, i, j);
            }
        }
    }
}
