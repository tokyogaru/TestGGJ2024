using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    private GameObject creditsMenu;
    private GameObject titleMenu;

    private void Start()
    {
        titleMenu = gameObject.transform.Find("Title").gameObject;
        creditsMenu = gameObject.transform.Find("Credits").gameObject;
    }

    public void ShowTitle()
    {
        titleMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }

    public void ShowCredits()
    {
        titleMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
