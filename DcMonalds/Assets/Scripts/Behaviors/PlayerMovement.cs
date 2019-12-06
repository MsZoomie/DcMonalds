using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Global Values")]

    [Tooltip("x is the left bound. y is the right bound.")]
    public Vector2 bounds = new Vector2(0, 5);
    
    private GameController gameController;
    private Swipe swipe;
    

    [Tooltip("Movement speed left and right.")]
    [Range(0, 10)]
    public float speedX = 1;
    [Tooltip("Movement speed forward.")]
    [Range(0, 10)]
    public float speedZ = 1.0f;


    private bool movingHorizontal;
    public Vector3 destination = Vector3.zero;


    private void Start()
    {
        swipe = GetComponent<Swipe>();
        gameController = FindObjectOfType<GameController>();
        destination.x = transform.position.x;
    }

    public void SetBounds(int left, int right)
    {
        bounds = new Vector2(left, right);
    }



    private void Update()
    {
        MoveForward();

        

        if (movingHorizontal)
        {
            Vector3 movementDelta = new Vector3(destination.x - transform.position.x, 0, 0) * Time.deltaTime * speedX;
            transform.position += movementDelta;


            if (Mathf.Abs(destination.x - transform.position.x)  <= 0.01)
            {
                transform.position = new Vector3(destination.x, transform.position.y, transform.position.z);
                movingHorizontal = false;
            }
        }
        else if (swipe.swipeLeft)
        {
            MoveHorizontal(-1);
        }
        else if (swipe.swipeRight)
        {
            MoveHorizontal(1);
        }
    }


    private void MoveForward()
    {
        transform.position += Vector3.forward * speedZ * Time.deltaTime;
    }


    private void MoveHorizontal(int direction)
    {

        if (direction == -1)
        {
            if (Mathf.Round(transform.position.x - 1) < bounds.x)
            {
                return;
            }
            
            destination.x = transform.position.x - 1;
            destination.x = Mathf.Round(destination.x);
        }
        else
        {
            if (Mathf.Round(transform.position.x + 1) > bounds.y)
            {
                return;
            }

            destination.x = transform.position.x + 1;

            destination.x = Mathf.Round(destination.x);
        }

        movingHorizontal = true;
    }

    



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Obstacle")
        {
            gameController.ChangeState(GameController.GameState.End);
        }
        
    }
}
