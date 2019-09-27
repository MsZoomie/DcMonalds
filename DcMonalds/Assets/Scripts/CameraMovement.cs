using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementScale = 1.0f;

    public Vector3 currentPos;
    public Vector3 nextPos;

    public Vector3 originalPos;
    public Vector3 normal;

    /*   bool moving;
       public float speed = 1.0f;
       private float startTime;
       private float journeyLength;*/


    private void Awake()
    {
        originalPos = transform.position;

        Vector3 temp = originalPos + Vector3.forward + Vector3.right;
        normal = Vector3.Cross(originalPos, temp);
    }

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
                    currentPos = touch.position;
                    currentPos = Camera.main.ScreenToWorldPoint(currentPos);
                    nextPos = touch.position;
                    nextPos = Camera.main.ScreenToWorldPoint(nextPos);
                    break;
                case TouchPhase.Moved:
                    currentPos = nextPos;
                    nextPos = touch.position;
                    nextPos = Camera.main.ScreenToWorldPoint(nextPos);

                    Vector2 temp = nextPos - currentPos;
                    Vector3 tempX = Vector3.right * temp.x;
                    tempX = Vector3.ProjectOnPlane(tempX, transform.up);

                    Vector3 tempY = Vector3.forward * temp.y;
                    tempY = Vector3.ProjectOnPlane(tempY, transform.right);

                    transform.position += tempX.normalized * movementScale;
                    transform.position += tempY.normalized * movementScale;

                    break;
                case TouchPhase.Ended:
            //        moving = false;
                    break;

                default:
                    break;
            }

        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            currentPos = Input.mousePosition;
            currentPos = Camera.main.ScreenToWorldPoint(currentPos);
            nextPos = Input.mousePosition;
            nextPos = Camera.main.ScreenToWorldPoint(nextPos);
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            currentPos = nextPos;
            nextPos = Input.mousePosition;
            nextPos = Camera.main.ScreenToWorldPoint(nextPos);

            Vector2 temp = nextPos - currentPos;
            //  Debug.Log(temp);

            Vector3 tempX = Vector3.right * temp.x;
            tempX = Vector3.ProjectOnPlane(tempX, transform.up);
            Debug.Log(tempX);

            Vector3 tempY = Vector3.forward * temp.y;
            tempY = Vector3.ProjectOnPlane(tempY, transform.right);

            transform.position += tempX.normalized * movementScale;
            transform.position += tempY.normalized * movementScale;

        }
     
    }

}
