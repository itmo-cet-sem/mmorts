using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControls : MonoBehaviour
{
    GameObject selectedUnit;

    [SerializeField]
    GameObject selection;

    [SerializeField]
    GameObject destination;

    [SerializeField]
    GameObject UnitInfo;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (GameLogic.GameManager.CurrentWorld.TempSelectedType != null)
            {
                MessageSender.SendSpawnMessage(GameLogic.GameManager.CurrentWorld.TempSelectedType.Name);
            }
        }
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
        {
            Vector3 clicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit;
            hit = Physics2D.Raycast(clicked, Vector2.down);
            if (hit)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.gameObject.tag.Equals("Unit"))
                    {
                        int uid = hit.collider.gameObject.GetComponent<UnitInfo>().ID;
                        Vector2Int sectorPosition = hit.collider.gameObject.GetComponent<UnitInfo>().Coordinates;

                        UnitInfo.SetActive(true);
                        UnitInfo.transform.GetChild(0).GetComponent<Text>().text = "Player: " + GameLogic.GameManager.CurrentWorld.Sectors[sectorPosition].Units[uid].Owner.Name;
                        UnitInfo.transform.GetChild(1).GetComponent<Text>().text = "Unit Type: " + GameLogic.GameManager.CurrentWorld.Sectors[sectorPosition].Units[uid].UnitType.ToString();

                        selection.GetComponent<SpriteRenderer>().enabled = true;
                        selectedUnit = hit.collider.gameObject;
                        selection.transform.position = selectedUnit.transform.position;
                        selection.transform.SetParent(selectedUnit.transform);
                        if (GameLogic.GameManager.CurrentWorld.Sectors[sectorPosition].Units[uid].Owner.Name == GameLogic.GameManager.CurrentPlayer.Name)
                        {
                            if (!GameLogic.GameManager.CurrentWorld.Sectors[sectorPosition].Units[uid].Destination.Equals(Vector3.negativeInfinity))
                            {
                                destination.transform.position = GameLogic.GameManager.CurrentWorld.Sectors[sectorPosition].Units[uid].Destination;
                                destination.GetComponent<SpriteRenderer>().enabled = true;
                            }
                            else
                            {
                                destination.GetComponent<SpriteRenderer>().enabled = false;
                            }
                        }
                    }
                    else
                    {
                        UnitInfo.SetActive(false);
                        selection.GetComponent<SpriteRenderer>().enabled = false;
                        destination.GetComponent<SpriteRenderer>().enabled = false;
                        selectedUnit = null;
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    if (selectedUnit != null)
                    {
                        int uid = selectedUnit.GetComponent<UnitInfo>().ID;
                        Vector2Int sectorPosition = selectedUnit.GetComponent<UnitInfo>().Coordinates;

                        if (GameLogic.GameManager.CurrentWorld.Sectors[sectorPosition].Units[uid].Owner.Name == GameLogic.GameManager.CurrentPlayer.Name)
                        {
                            if (hit.point != null)
                            {
                                GameLogic.GameManager.CurrentWorld.Sectors[sectorPosition].Units[uid].Destination = hit.point;
                                MessageSender.SendMoveMessage(selectedUnit.GetComponent<UnitInfo>().ID, GameLogic.GameManager.Space, GameLogic.GameManager.CurrentWorld.Sectors[sectorPosition]);
                                destination.transform.position = GameLogic.GameManager.CurrentWorld.Sectors[sectorPosition].Units[uid].Destination;
                                destination.GetComponent<SpriteRenderer>().enabled = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
