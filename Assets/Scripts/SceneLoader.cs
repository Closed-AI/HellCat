using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//скрипт для перехода между сценами
// (секция try - catch предназначена для отлова ошибок названия сцены)
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string SceneName)
    {
        try
        {
            SceneManager.LoadScene(SceneName);
        }
        catch
        {
            Debug.LogError("Сцены с таким названием не существует! Проверьте название!!!");
        }
    }
}
