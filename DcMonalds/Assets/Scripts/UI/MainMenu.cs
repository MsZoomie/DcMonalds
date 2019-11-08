using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;

    private void Start()
    {
        Sprite[] levelThumbnails = Resources.LoadAll<Sprite>("Prefabs/UI/Levels");
        foreach (Sprite sprite in levelThumbnails)
        {
            GameObject button = Instantiate(levelButtonPrefab);
            button.GetComponent<Image>().sprite = sprite;
            button.transform.SetParent(levelButtonContainer.transform, false);

            string sceneName = sprite.name;
            button.GetComponent<Button>().onClick.AddListener(() => LoadScene(sceneName));
        }
    }


    public void LookAtMenu(Transform menuTransform)
    {
        Camera.main.transform.LookAt(menuTransform);
    }


    public override void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
