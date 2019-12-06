using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    private GameController gameController;

    public override void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }




    private void Awake()
    {
   //     gameController = FindObjectOfType<GameController>();
    }


    public void StartGame()
    {
  //      gameController.ChangeState(GameController.GameState.Play);
    }

    public void PauseGame()
    {
   //     gameController.ChangeState(GameController.GameState.Pause);
    }

    public void RestartGame()
    {
     //   gameController.ChangeState(GameController.GameState.Start);
    }
}
