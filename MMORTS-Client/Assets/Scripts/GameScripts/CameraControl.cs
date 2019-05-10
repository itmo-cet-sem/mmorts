using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    public bool IsInMenu = false;
    [SerializeField]
    Text coords;

    float speed = 0.1f;
    const float speedUpMultiplayer = 5f;
    float currentSpeedUpMultiplayer;
    float wheelSpeed = 10f;
    Vector2Int currentSector;

    private void Start()
    {
        currentSector = new Vector2Int((int)transform.position.x / 10, (int)transform.position.y / 10);
        coords.text = "X: " + currentSector.x.ToString() + "\nY: " + currentSector.y.ToString();
        selectVisibleSectors();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsInMenu)
        {
            if (Input.GetAxis("SpeedUp")!=0)
            {
                currentSpeedUpMultiplayer = speedUpMultiplayer;
            }
            else
            {
                currentSpeedUpMultiplayer = 1;
            }
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Vector3 newPosition = transform.position;
                newPosition.x += Input.GetAxis("Horizontal") * speed * currentSpeedUpMultiplayer;
                newPosition.y += Input.GetAxis("Vertical") * speed * currentSpeedUpMultiplayer;
                transform.position = newPosition;
                processSectors();
            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    if (Camera.main.orthographicSize < 20)
                    {
                        Camera.main.orthographicSize -= wheelSpeed * Input.GetAxis("Mouse ScrollWheel") * currentSpeedUpMultiplayer;
                    }
                }
                else
                {
                    if (Camera.main.orthographicSize > 1)
                    {
                        Camera.main.orthographicSize -= wheelSpeed * Input.GetAxis("Mouse ScrollWheel") * currentSpeedUpMultiplayer;
                    }
                }
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
                processSectors();
            }
        }
    }

    private void processSectors()
    {
        Vector2Int newSector = new Vector2Int((int)transform.position.x/ Config.SectorSize, (int)transform.position.y/Config.SectorSize);
        coords.text = "X: " + newSector.x.ToString() + "\nY: " + newSector.y.ToString();
        if (newSector != currentSector)
        {
            currentSector = newSector;
            foreach (Vector2Int key in GameLogic.GameManager.CurrentWorld.Sectors.Keys)
            {
                GameLogic.GameManager.CurrentWorld.Sectors[key].IsActive = false;
            }
            selectVisibleSectors();
        }
    }

    private void selectVisibleSectors()
    {
        for (int i = -Config.FieldOfViewRadius; i < Config.FieldOfViewRadius; i++)
        {
            for (int j = -Config.FieldOfViewRadius; j < Config.FieldOfViewRadius; j++)
            {
                Vector2Int sector = new Vector2Int(currentSector.x + i, currentSector.y + j);
                if (GameLogic.GameManager.CurrentWorld.Sectors.ContainsKey(sector))
                {
                    GameLogic.GameManager.CurrentWorld.Sectors[sector].IsActive = true;
                }
                else
                {
                    GameLogic.GameManager.CurrentWorld.Sectors.Add(sector, new Sector(sector));
                    SectorGridCreation.CreateGridTexture(sector);
                }
            }
        }
    }
}
