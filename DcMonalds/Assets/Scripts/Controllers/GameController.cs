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

    public int numberOfLanes;
    

    private PlayerMovement playerMovement;
    private PlayerBehaviour playerBehaviour;

    public UIController UIcontroller;
    public SceneController sceneController;


    EndlessLevelGenerator levelGenerator;

    private void Awake()
    {
        currentState = GameState.Start;

        playerMovement = FindObjectOfType<PlayerMovement>();
        playerBehaviour = playerMovement.gameObject.GetComponent<PlayerBehaviour>();
       
        levelGenerator = GetComponent<EndlessLevelGenerator>();
    }

    private void Start()
    {
        playerMovement.SetBounds(0, numberOfLanes);
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
                playerMovement.enabled = false;
                playerBehaviour.Jumping(false);
                
                StartCoroutine(WaitBeforeMenu(GameState.Pause));

                levelGenerator.enabled = false;

                break;

            case GameState.Resume:
                playerMovement.enabled = true;
                playerBehaviour.Jumping(true);

                levelGenerator.enabled = true;

                break;

            case GameState.End:
                playerMovement.enabled = false;
                StartCoroutine(WaitBeforeMenu(GameState.End));

                levelGenerator.enabled = false;
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


    private IEnumerator WaitBeforeMenu(GameState state)
    {
        yield return new WaitForSeconds(1);

        switch (state)
        {
            case GameState.Pause:
                UIcontroller.Pause();
                break;
            case GameState.End:
                UIcontroller.Lose();
                break;
            default:
                break;
        }
    }
}
