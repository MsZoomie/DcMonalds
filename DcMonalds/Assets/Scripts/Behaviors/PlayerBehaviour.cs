using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private GameController gameController;


    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }


    private void OnCollisionEnter(Collision collision)
    {        
        if(collision.gameObject.tag == "Obstacle")
        {
            gameController.ChangeState(GameController.GameState.End);
        }
    }
}
