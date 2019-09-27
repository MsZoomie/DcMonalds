using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private void LateUpdate()
    {
        Move();

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    private void Move()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {

                Vector2 pos = touch.position;
                transform.position = new Vector3(-pos.x, pos.y, 0.0f);
            }
        }
    }

}
