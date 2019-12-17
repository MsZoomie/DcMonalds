using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public float delay = 0;
    public int distanceToTravel;
    public float speed = 1.0f;

    private Movement movement;
   
    private void Awake()
    {
        movement = gameObject.GetComponent<Movement>();
        SetMovementValues();
    }

    private void Start()
    {
        StartCoroutine( WaitForDelay());
        
    }
    private void SetMovementValues()
    {
        Vector3 temp = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        movement.SetStartMarker(temp);
        temp = new Vector3(transform.position.x, transform.position.y, transform.position.z - distanceToTravel);
        movement.SetEndMarker(temp);
        movement.alwaysLerping = false;
        movement.turn = false;
        movement.lerpSpeed = speed;
    }

    IEnumerator WaitForDelay()
    {
        yield return new WaitForSeconds(delay);
        movement.StartLerp(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            movement.enabled = false;
            this.enabled = false;
        }
    }
}
