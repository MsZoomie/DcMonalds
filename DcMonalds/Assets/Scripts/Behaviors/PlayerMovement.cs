using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("x is the left bound. y is the right bound.")]
    public Vector2 bounds = new Vector2(0, 5);
    [Tooltip("Movement speed left and right.")]
    [Range(0, 1)]
    public float speedX = 1.0f;
    [Tooltip("Movement speed forward.")]
    [Range(0, 10)]
    public float speedZ = 1.0f;

    private float direction;
    public bool moving;
    
    private GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {

        MoveForward();
        if (moving)
        {
            MoveHorizontal();
        }

    }


    private void MoveForward()
    {
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
        if (collision.collider.tag == "Obstacle")
        {
            gameController.ChangeState(GameController.GameState.End);
        }
        
    }
}
