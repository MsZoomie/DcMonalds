using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }


    public void StartGame()
    {
        gameController.ChangeState(GameController.GameState.Play);
    }

    public void PauseGame()
    {
        gameController.ChangeState(GameController.GameState.Pause);
    }

    public void RestartGame()
    {
        gameController.ChangeState(GameController.GameState.Start);
    }





    public void Quit()
    {
        Application.Quit();
    }
}
