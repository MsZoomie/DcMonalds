using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum Direction
    { LEFT, RIGHT, FORWARD }
    public enum MovementMethod
    { NoInput, CrossyRoad, AlwaysForward}

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
        
        if (turning)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, turnTimeCount);
            turnTimeCount += Time.deltaTime;

            if (Mathf.Abs( transform.rotation.y - endRot.y) <= 0.01)
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

            if (Vector3.Distance( transform.position, endPos) <= 0.01 )
            {
                moving = false;
            }
        }
        else
        {
            switch (movementMethod)
            {
                case MovementMethod.CrossyRoad:
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        CrossyRoadMove(Direction.LEFT);
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        CrossyRoadMove(Direction.RIGHT);
                    }
                    else if (Input.GetKeyDown(KeyCode.W))
                    {
                        CrossyRoadMove(Direction.FORWARD);
                    }
                    break;
                default:
                    break;
            }
        }

    }
    private void Walk(Direction direction)
    {
        moving = true;
        movingStartTime = Time.time;
        startPos = transform.position;

        switch (direction)
        {
            case Direction.LEFT:
                endPos = new Vector3(startPos.x - 1, startPos.y, startPos.z);
                break;
            case Direction.RIGHT:
                endPos = new Vector3(startPos.x + 1, startPos.y, startPos.z);
                break;
            case Direction.FORWARD:
                endPos = new Vector3(startPos.x, startPos.y, startPos.z + 1);
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
            case Direction.LEFT:
                endRot = Quaternion.AngleAxis(-90.0f, Vector3.up);
                break;
            case Direction.RIGHT:
                endRot = Quaternion.AngleAxis(90.0f, Vector3.up);
                break;
            case Direction.FORWARD:
                endRot = Quaternion.AngleAxis(0.0f, Vector3.up);
                break;
            default:
                break;
        }
    }

    private void CrossyRoadMove(Direction dir)
    {
        Walk(dir);
        Turn(dir);
    }

    public void Move(Direction dir)
    {
        switch (movementMethod)
        {
            case MovementMethod.CrossyRoad:
                CrossyRoadMove(dir);
                break;
            default:
                break;
        }
    }
   

    private bool CheckWalkablity(Vector3 tilePos)
    {
        GameObject tile = searchSpace.Tiles.Find 
               (t => Mathf.Abs(t.transform.position.x - tilePos.x) <= 0.1f
               && Mathf.Abs(t.transform.position.z - tilePos.z) <= 0.1f );

        if (tile != null)
        {
            return tile.GetComponent<Tile>().GetWalkability();
        }
        else
        {
            Debug.Log("There's no tile there.");
            return false;
        }
    }
}
