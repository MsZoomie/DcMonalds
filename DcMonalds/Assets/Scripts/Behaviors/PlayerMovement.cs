using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum MovementType
    {
        DcMonalds, CrossyRoad
    }

    public MovementType movementType;

    [Header("Global Values")]

    [Tooltip("x is the left bound. y is the right bound.")]
    public Vector2 bounds = new Vector2(0, 5);
    public bool moving;
    private float direction;

    private GameController gameController;
    

    [Header("DcMonalds Values")]
    [Tooltip("Movement speed left and right.")]
    [Range(0, 1)]
    public float speedX = 1.0f;
    [Tooltip("Movement speed forward.")]
    [Range(0, 10)]
    public float speedZ = 1.0f;


    [Header("CrossyRoadValues")]
    public float speed = 1.0f;
    private Vector3 startPos;
    private Vector3 endPos;


    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void SetBounds(int left, int right)
    {
        bounds = new Vector2(left, right);
    }

    private void Update()
    {
        switch (movementType)
        {
            case MovementType.DcMonalds:
                MoveForward();
                if (moving)
                {
                    MoveHorizontal();
                }
                break;

            case MovementType.CrossyRoad:
                if (Input.GetKey(KeyCode.A))
                {
                    MoveLeft();
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    MoveRight();
                }
                else if(Input.GetKey(KeyCode.W))
                {
                    MoveForward();
                }
                break;
            
            default:
                break;
        }
    }


    private void MoveForward()
    {
        switch (movementType)
        {
            case MovementType.DcMonalds:
                Vector3 forward = Vector3.forward * speedZ;
                transform.position += forward * Time.deltaTime;
                break;

            case MovementType.CrossyRoad:
                startPos = transform.position;
                endPos = startPos + Vector3.forward;
                float startTime = Time.time;
                float journeyFraction = 1 / Vector3.Distance(transform.position, endPos);

                while(journeyFraction >= 0.0001)
                {
                    journeyFraction = (Time.time - startTime) * speed;
                    transform.position = Vector3.Lerp(startPos, endPos, journeyFraction);
                }
                
                break;

            default:
                break;
        }
    }

    private void MoveHorizontal()
    {
        switch (movementType)
        {
            case MovementType.DcMonalds:
                Vector3 horizontal = new Vector3(direction * speedX, 0, 0);

                transform.position += horizontal;
                float x = Mathf.Clamp(transform.position.x, bounds.x, bounds.y);

                transform.position = new Vector3(x, transform.position.y, transform.position.z);
                break;

            case MovementType.CrossyRoad:
                startPos = transform.position;
                endPos = startPos + Vector3.right * direction;
                float startTime = Time.time;
                float journeyFraction = 1 / Vector3.Distance(transform.position, endPos);

                while(journeyFraction >= 0.0001)
                {
                    journeyFraction = (Time.time - startTime) * speed;
                    transform.position = Vector3.Lerp(startPos, endPos, journeyFraction);
                }
                break;


            default:
                break;
        }
        
    }

    
    public void MoveLeft()
    {
        moving = true;
        direction = -1;
        
    }

    public void MoveRight()
    {
        moving = true;
        direction = 1;
    }

    public void StopMoving()
    {
        moving = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Obstacle")
        {
            gameController.ChangeState(GameController.GameState.End);
        }
        
    }
}
