using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementScale = 1.0f;

    private Vector3 plane = new Vector3(1, 0, 1);
    private Vector3 planeNormal;
    private float distanceToPlane;

    private Vector2 currentPos;
    private Vector2 nextPos;

    private void Awake()
    {/*
        planeNormal = Vector3.Cross(plane, new Vector3(1, 0, 2));
        Vector3 positionProj = Vector3.Project(transform.position, planeNormal);
        distanceToPlane = Vector3.Distance(positionProj, plane);*/
    }

    private void Update()
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
                    movementScale *= 0.1f;
                    if (temp.x > 0)
                    {
                        MoveLeft();
                    }
                    else if (temp.x < 0)
                    {
                        MoveRight();
                    }

                    if (temp.y > 0)
                    {
                        MoveDown();
                    }
                    else if (temp.y < 0)
                    {
                        MoveUp();
                    }
                    movementScale *= 10f;

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

            movementScale *= 0.1f;
            if (temp.x > 0)
            {
                MoveLeft();
            }
            else if (temp.x < 0)
            {
                MoveRight();
            }

            if (temp.y > 0)
            {
                MoveDown();
            }
            else if (temp.y < 0)
            {
                MoveUp();
            }

            movementScale *= 10f;

        }
    }



    public void MoveUp()
    {
        transform.Translate(Vector3.up * movementScale);
    }

    public void MoveDown()
    {
        transform.Translate(Vector3.down * movementScale);
    }

    public void MoveRight()
    {
        transform.Translate(Vector3.right * movementScale);
    }

    public void MoveLeft()
    {
        transform.Translate(Vector3.left * movementScale);
    }
}
