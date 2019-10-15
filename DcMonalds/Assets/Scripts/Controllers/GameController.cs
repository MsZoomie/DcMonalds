using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
       Start, Play, Pause, End
    }

    public GameState currentState;

    private PlayerMovement playerMovement;
    private GameObject player;
    private Vector3 playerStartPos;
    private Rigidbody playerRB;

    public GameObject movementUI;
    public GameObject startUI;
    public GameObject pauseUI;
    public GameObject pauseButton;
    public GameObject endUI;

    public Level level;


    private void Awake()
    {
        currentState = GameState.Start;
        playerMovement = FindObjectOfType<PlayerMovement>();
        player = playerMovement.gameObject;
        playerStartPos = player.transform.position;
        playerRB = player.GetComponent<Rigidbody>();
        level = FindObjectOfType<Level>();
    }

    private void Start()
    {
        playerMovement.SetBounds(0, level.GetNumberOfLanes());
        playerMovement.enabled = false;
    }


    public void ChangeState(GameState newState)
    {
        ExitState(currentState);
        EnterState(newState);
    }


    private void EnterState(GameState gameState)
    {
        currentState = gameState;

        switch (gameState)
        {
            case GameState.Start:

                pauseButton.SetActive(false);
                playerRB.velocity = Vector3.zero;
                playerRB.angularVelocity = Vector3.zero;
                playerRB.rotation = Quaternion.identity;
                //playerRB.ResetCenterOfMass();
                player.transform.position = playerStartPos;

                ChangeState(GameState.Play);
                break;
            case GameState.Pause:
                pauseUI.SetActive(true);
                pauseButton.SetActive(false);
                break;
            case GameState.Play:
                playerMovement.enabled = true;
                movementUI.SetActive(true);
                pauseButton.SetActive(true);
                break;
            case GameState.End:
                endUI.SetActive(true);

                pauseButton.SetActive(false);
                break;
            default:
                break;
        }
    }
    
    private void ExitState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Start:
                startUI.SetActive(false);
                break;
            case GameState.Play:
                movementUI.SetActive(false);
                playerMovement.moving = false;
                playerMovement.enabled = false;
                break;
            case GameState.Pause:
                pauseUI.SetActive(false);
                
                
                break;
            case GameState.End:
                endUI.SetActive(false);
                break;
            default:
                break;
        }
    }
}
