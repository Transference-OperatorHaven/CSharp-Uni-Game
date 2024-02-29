using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(LoadGame());
    }

    public void StartTutorial()
    {
        StartCoroutine(LoadTutorial());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadTutorial()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Tutorial");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
