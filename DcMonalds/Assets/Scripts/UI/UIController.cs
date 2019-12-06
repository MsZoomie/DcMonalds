using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject[] hideInPause;
    [SerializeField] private GameObject[] showInPause;
    [SerializeField] private GameObject[] showDuringPlay;


    public void Pause()
    {
        foreach (var item in hideInPause)
        {
            item.SetActive(false);
        }

        foreach (var item in showInPause)
        {
            item.SetActive(true);
        }
    }

    public void Unpause()
    {
        foreach (var item in hideInPause)
        {
            item.SetActive(true);
        }

        foreach (var item in showInPause)
        {
            item.SetActive(false);
        }
    }


    public void Play()
    {
        foreach (var item in showDuringPlay)
        {
            item.SetActive(true);
        }
    }

    public void HideAll()
    {
        foreach (var item in hideInPause)
        {
            item.SetActive(false);
        }

        foreach (var item in showInPause)
        {
            item.SetActive(false);
        }

        foreach (var item in showDuringPlay)
        {
            item.SetActive(false);
        }
    }
}
