using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    public SceneController sceneController;

    public GameObject levelButtonPrefab;
    public GameObject levelButtonsContainer;

    private Sprite[] thumbnails;

    private void Start()
    {
        thumbnails = Resources.LoadAll<Sprite>("Prefabs/UI/Levels");
        foreach (Sprite thumbnail in thumbnails)
        {
            GameObject container = Instantiate<GameObject>(levelButtonPrefab);
            container.GetComponent<Image>().sprite = thumbnail;
            container.transform.SetParent(levelButtonsContainer.transform, false);


            string sceneName = thumbnail.name;
            container.GetComponent<Button>().onClick.AddListener(() => sceneController.LoadScene(sceneName));
        }
    }



    public void LookAtMenu(Transform menu)
    {
        Camera.main.transform.LookAt(menu.position);
    }
}
