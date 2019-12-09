using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Animator playerAnimator;

    private GameController gameController;


    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        playerAnimator = GetComponent<Animator>();

        Jumping(true);
    }


    private void OnCollisionEnter(Collision collision)
    {        
        if(collision.gameObject.tag == "Obstacle")
        {
            Jumping(false);
            gameController.ChangeState(GameController.GameState.End);

            Falling();
        }
    }

    public void Jumping(bool jumping)
    {
        playerAnimator.SetBool("jump", jumping);
    }

    public void Falling()
    {
        playerAnimator.SetTrigger("falling");
    }
}
