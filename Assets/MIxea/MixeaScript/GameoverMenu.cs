using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverMenu : MonoBehaviour
{
    public GameObject endMenu;

    private static bool isOver;

    // Start is called before the first frame update
    void Start()
    {
        endMenu.SetActive(false);

        isOver = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseGame()
    {
        endMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResetScene()
    {
        Time.timeScale = 1f;
        isOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
