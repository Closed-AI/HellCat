using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject menuUI;
    public GameObject countdown;

    private const string META = "MetaGameplay";

    public void OnClickContinue()
    {
        menuUI.SetActive(false);
        gameUI.SetActive(true);
        countdown.SetActive(true);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene(META);
    }

    public void OnClickSound()
    {
        // Прописать, когда будет звук
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
    }

    public void OnClickMenu()
    {
        Time.timeScale = 0;
        menuUI.SetActive(true);
        gameUI.SetActive(false);
    }
}
