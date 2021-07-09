using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//скрипт для перехода между сценами
// (секция try - catch предназначена для отлова ошибок названия сцены)
public class SceneManager : MonoBehaviour
{
    public void LoadScene(string SceneName)
    {
        try
        {
            Application.LoadLevel(SceneName);
        }
        catch
        {
            Debug.LogError("Сцены с таким названием не существует! Проверьте название!!!");
        }
    }
}
