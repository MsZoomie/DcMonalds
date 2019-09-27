using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    Vector3 startPos;
    Vector3 currentPos;
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

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    currentPos = touch.position;

                    transform.position = new Vector3(currentPos.x, transform.position.y, currentPos.z);

                    break;
                case TouchPhase.Ended:
                    break;

                default:
                    break;
            }

        }
    }

}
