using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LoadGameScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MessageSender.StartingGetUnitInfo();
        GameLogic.GameManager.CurrentWorld = new GameLogic.World();
        GameLogic.GameManager.Players = new List<GameLogic.Player>();
        GameLogic.GameManager.Players.Add(GameLogic.GameManager.CurrentPlayer);
        GameLogic.GameManager.LastState = 0;
        MessageSender.SendGetUnitTypesMessage();
    }
}
