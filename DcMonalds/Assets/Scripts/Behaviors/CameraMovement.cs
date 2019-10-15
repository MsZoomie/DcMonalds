using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool developmentMode = true;
    [Tooltip("When moving the camera in development mode")]
    public float movementScale = 1.0f;

    [Space]
    [Header("For following player")]
    public GameObject player;
    private PlayerMovement playerMovement;
    public float offset = 0;
  
    [Space]
    [Header("For CrossyRoad")]
    public float speed = 1.0f;
    

    private Vector3 plane = new Vector3(1, 0, 1);
    private Vector3 planeNormal;
    private float distanceToPlane;

    private Vector2 currentPos;
    private Vector2 nextPos;

    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }
    

    private void LateUpdate()
    {
        if (developmentMode)
        {
            //on mobile
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


            //on computer
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
        else
        {
            switch (playerMovement.movementType)
            {
                case PlayerMovement.MovementType.DcMonalds:
                    transform.position = new Vector3 (transform.position.x, transform.position.y, player.transform.position.z + offset);
                    break;
                case PlayerMovement.MovementType.CrossyRoad:
                    transform.position += Vector3.forward * speed * Time.deltaTime;
                    break;
                default:
                    break;
            }
            
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
