using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    Vector3 startPos;
    Vector3 endPos;

 /*   bool moving;
    public float speed = 1.0f;
    private float startTime;
    private float journeyLength;*/


   
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
            //        moving = true;
            //        startTime = Time.time;
                    break;
                case TouchPhase.Moved:
                    endPos = touch.position;

                    transform.position += new Vector3(endPos.x - startPos.x, transform.position.y, endPos.z - startPos.z);


                    break;
                case TouchPhase.Ended:
            //        moving = false;
                    break;

                default:
                    break;
            }

        }
    }

}
