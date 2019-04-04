using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour
{
    GameObject selectedUnit;

    [SerializeField]
    GameObject selection;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            MessageSender.SendSpawnMessage(GameLogic.UnitTypes.Circle);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            MessageSender.SendSpawnMessage(GameLogic.UnitTypes.Square);
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
                        if (GameLogic.GameManager.CurrentWorld.Units[uid].Owner.Name == GameLogic.GameManager.CurrentPlayer.Name)
                        {
                            selection.GetComponent<SpriteRenderer>().enabled = true;
                            selectedUnit = hit.collider.gameObject;
                            selection.transform.position = selectedUnit.transform.position;
                            selection.transform.SetParent(selectedUnit.transform);
                        }
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    if (selectedUnit != null)
                    {
                        int uid = selectedUnit.GetComponent<UnitInfo>().ID;
                        GameLogic.GameManager.CurrentWorld.Units[uid].Destanation = hit.point;
                        MessageSender.SendMoveMessage(selectedUnit.GetComponent<UnitInfo>().ID);
                    }
                }
            }
        }
    }
}
