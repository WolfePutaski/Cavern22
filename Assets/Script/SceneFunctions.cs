using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFunctions : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName(sceneName).buildIndex);
    }

    public void GoToScene(int sceneInt)
    {
        SceneManager.LoadScene(sceneInt);
    }


    public void quitGame()
    {
        Application.Quit();
    }
}
