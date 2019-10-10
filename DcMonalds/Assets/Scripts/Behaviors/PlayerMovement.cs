using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum Direction
    { Left, Right, Forward, LeftAndForward, RightAndForward }
    public enum MovementMethod
    { NoInput, Dancing, AlwaysForward, DcMonalds}

    public bool tiledEnviroment;
    private SearchSpace searchSpace;

    [Range(0, 10)]
    public float movementSpeed = 1.0f;
    [Range(0.0f, 1.0f)]
    public float turnSpeed = 0.1f;

    private float movingStartTime;
    private float turnTimeCount;
    private float journeyLength;

    public MovementMethod movementMethod;
    public bool moving;
    public bool turning;


    [Space]
    [Space]
    [Header("DcMonalds Player")]
    public Vector2 bounds = new Vector2(0, 5);
    [Range(0, 1)]
    public float speedX = 1.0f;
    [Range(0, 10)]
    public float speedZ = 1.0f;

    private float direction;

   


    //lerp stuff
    private Vector3 startPos;
    private Vector3 endPos;

    private Quaternion startRot;
    private Quaternion endRot;

    private void Awake()
    {
        if (tiledEnviroment)
        {
            searchSpace = FindObjectOfType<SearchSpace>();
        }
    }


    private void Update()
    {
        switch (movementMethod)
        {
            case MovementMethod.Dancing:
                {
                    if (turning)
                    {
                        transform.rotation = Quaternion.Slerp(startRot, endRot, turnTimeCount);
                        turnTimeCount += Time.deltaTime;

                        if (Mathf.Abs(transform.rotation.y - endRot.y) <= 0.01)
                        {
                            turning = false;
                            if (moving)
                            {
                                movingStartTime = Time.time;
                                startPos = transform.position;
                            }
                        }
                    }

                    if (moving)
                    {
                        float distCovered = (Time.time - movingStartTime) * movementSpeed;
                        float fracJourney = distCovered / journeyLength;
                        transform.position = Vector3.Lerp(startPos, endPos, fracJourney);

                        if (Vector3.Distance(transform.position, endPos) <= 0.01)
                        {
                            moving = false;
                        }
                    }
                    else
                    {

                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            MoveDancing(Direction.Left);
                        }
                        else if (Input.GetKeyDown(KeyCode.D))
                        {
                            MoveDancing(Direction.Right);
                        }
                        else if (Input.GetKeyDown(KeyCode.W))
                        {
                            MoveDancing(Direction.Forward);
                        }
                    }
                }
                break;

            case MovementMethod.AlwaysForward:
                {
                    if (moving)
                    {
                        float distCovered = (Time.time - movingStartTime) * movementSpeed;
                        float fracJourney = distCovered / journeyLength;
                        transform.position = Vector3.Lerp(startPos, endPos, fracJourney);

                        if (Vector3.Distance(transform.position, endPos) <= 0.01)
                        {
                            moving = false;
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            MoveAlwaysForward(Direction.LeftAndForward);
                        }
                        else if (Input.GetKeyDown(KeyCode.D))
                        {
                            MoveAlwaysForward(Direction.RightAndForward);
                        }
                        else
                            MoveAlwaysForward(Direction.Forward);
                    }
                }
                break;

            case MovementMethod.DcMonalds:
                {
                    MoveDcMonalds(Input.GetAxis("Horizontal"));
                    if (moving)
                    {
                        MoveHorizontal();
                    }
                }
                break;
            default:
                break;
        }
    }
    private void Walk(Direction direction)
    {
        moving = true;
        movingStartTime = Time.time;
        startPos = transform.position;

        switch (direction)
        {
            case Direction.Left:
                endPos = new Vector3(startPos.x - 1, startPos.y, startPos.z);
                break;
            case Direction.Right:
                endPos = new Vector3(startPos.x + 1, startPos.y, startPos.z);
                break;
            case Direction.Forward:
                endPos = new Vector3(startPos.x, startPos.y, startPos.z + 1);
                break;
            case Direction.LeftAndForward:
                endPos = new Vector3(startPos.x - 1, startPos.y, startPos.z + 1);
                break;
            case Direction.RightAndForward:
                endPos = new Vector3(startPos.x + 1, startPos.y, startPos.z + 1);
                break;
            default:
                break;
        }

        if (!CheckWalkablity(endPos))
        {
            moving = false;
            return;
        }

        journeyLength = Vector3.Distance(startPos, endPos);
    }
    
    public void Turn(Direction direction)
    {
        turning = true;
        startRot = transform.rotation;
        turnTimeCount = 0.0f;

        switch (direction)
        {
            case Direction.Left:
                endRot = Quaternion.AngleAxis(-90.0f, Vector3.up);
                break;
            case Direction.Right:
                endRot = Quaternion.AngleAxis(90.0f, Vector3.up);
                break;
            case Direction.Forward:
                endRot = Quaternion.AngleAxis(0.0f, Vector3.up);
                break;
            default:
                break;
        }
    }

    private void MoveDancing(Direction dir)
    {
        Walk(dir);
        Turn(dir);
    }
    
    private void MoveAlwaysForward(Direction dir)
    {
        Walk(dir);
    }


    public void Move(Direction dir)
    {
        switch (movementMethod)
        {
            case MovementMethod.Dancing:
                MoveDancing(dir);
                break;
            case MovementMethod.AlwaysForward:
                break;
            case MovementMethod.NoInput:
                MoveDancing(dir);
                break;
            default:
                break;
        }
    }
   

    private bool CheckWalkablity(Vector3 tilePos)
    {
        Tile tile = searchSpace.tiles.Find 
               (t => Mathf.Abs(t.gameObject.transform.position.x - tilePos.x) <= 0.1f
               && Mathf.Abs(t.gameObject.transform.position.z - tilePos.z) <= 0.1f );

        if (tile != null)
        {
            return tile.GetWalkability();
        }
        else
        {
            Debug.Log("There's no tile there.");
            return false;
        }
    }




    private void MoveDcMonalds(float dir)
    {
            Vector3 horizontal = new Vector3(dir * speedX, 0, 0);
           
            transform.position += horizontal;
            float x = Mathf.Clamp(transform.position.x, bounds.x, bounds.y);

            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        

        Vector3 forward = Vector3.forward * speedZ;
        transform.position += forward * Time.deltaTime;
    }

    private void MoveHorizontal()
    {
        Vector3 horizontal = new Vector3(direction * speedX, 0, 0);

        transform.position += horizontal;
        float x = Mathf.Clamp(transform.position.x, bounds.x, bounds.y);

        transform.position = new Vector3(x, transform.position.y, transform.position.z);
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
        
    }
}
