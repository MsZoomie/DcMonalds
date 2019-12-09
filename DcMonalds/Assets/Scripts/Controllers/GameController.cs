using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
       Start, Resume, Pause, End, Restart
    }

    public GameState currentState;


    private Level level;

    private PlayerMovement playerMovement;
    private PlayerBehaviour playerBehaviour;

    public UIController UIcontroller;
    public SceneController sceneController;

    private void Awake()
    {
        currentState = GameState.Start;

        playerMovement = FindObjectOfType<PlayerMovement>();
        playerBehaviour = playerMovement.gameObject.GetComponent<PlayerBehaviour>();
       
        level = FindObjectOfType<Level>();
    }

    private void Start()
    {
        playerMovement.SetBounds(0, level.GetNumberOfLanes());
        playerMovement.enabled = false;
        EnterState(currentState);
    }


    public void Pause()
    {
        ChangeState(GameState.Pause);
    }

    public void Resume()
    {
        ChangeState(GameState.Resume);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
		    Application.Quit();
        #endif
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
                UIcontroller.HideAll();
                UIcontroller.Play();

                ChangeState(GameState.Resume);
                break;

            case GameState.Pause:
                UIcontroller.Pause();
                playerMovement.enabled = false;
                playerBehaviour.Jumping(false);
                
                break;

            case GameState.Resume:
                playerMovement.enabled = true;
                playerBehaviour.Jumping(true);
                break;

            case GameState.End:
                playerMovement.enabled = false;
                UIcontroller.Lose();
                break;

            case GameState.Restart:
                sceneController.ReloadScene();
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
                
                break;

            case GameState.Resume:
                
                break;

            case GameState.Pause:
                UIcontroller.Unpause();
                break;

            case GameState.End:
              
                break;
            default:
                break;
        }
    }
}
