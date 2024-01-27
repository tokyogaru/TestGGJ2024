using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManaging : MonoBehaviour
{
    public void NextScene()
    {
        Debug.Log("Next Scene");

        SceneManager.LoadScene("MixeaScene2");

    }
}
