using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject menuButton;
    public GameObject joystick;
    public GameObject dashButton;
    public GameObject screenPanels;

    public void OnClickContinue()
    {
        Time.timeScale = 1;
        screenPanels.SetActive(false);
        menuButton.SetActive(true);
        joystick.SetActive(true);
        dashButton.SetActive(true);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickMainMenu()
    {
        //Time.timeScale = 1;
        //SceneManager.LoadScene("сцена с меню");
    }

    public void OnClickSound()
    {
        // Прописать, когда будет звук
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickMenu()
    {
        Time.timeScale = 0;
        screenPanels.SetActive(true);
        menuButton.SetActive(false);
        joystick.SetActive(false);
        dashButton.SetActive(false);
    }
}
