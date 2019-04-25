using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    float speed = 0.1f;
    float wheelSpeed = 10f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical")!=0)
        {
            Vector3 newPosition = transform.position;
            newPosition.x += Input.GetAxis("Horizontal") *speed;
            newPosition.y += Input.GetAxis("Vertical") * speed;
            transform.position = newPosition;
        }
        if (Input.GetAxis("Mouse ScrollWheel") !=0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Camera.main.orthographicSize < 100)
                {
                    Camera.main.orthographicSize -= wheelSpeed * Input.GetAxis("Mouse ScrollWheel");
                }
            }
            else
            {
                if (Camera.main.orthographicSize > 1)
                {
                    Camera.main.orthographicSize -= wheelSpeed * Input.GetAxis("Mouse ScrollWheel");
                }
            }
        }
    }
}
