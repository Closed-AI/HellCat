using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private UnityAction PatternCompleted;

    [SerializeField] private Text text;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private PatternSpawner  patternSpawner;
    [SerializeField] private float scoreSpeed; // очков в секунду (базовое значение, которое будет взаимодействовать с множителем счёта)
    [SerializeField] private int maxScore;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        PatternCompleted += OnPatternCompleted;
        // спавн начинается только при создании нового объекта
        obstacleSpawner = Instantiate(obstacleSpawner);
        score = 0;
        scoreSpeed = 1f / scoreSpeed;
        StartCoroutine(addScore());
        text.text = "";
    }

    private IEnumerator addScore()
    {
        while (score < maxScore)
        {
            score++;
            progressBar.GetComponent<ProgressBarController>().SetVal((float)score / maxScore);
            yield return new WaitForSeconds(scoreSpeed);
        }

        obstacleSpawner.StopSpawn();
        text.text = "Danger";
        yield return new WaitForSeconds(1.5f);
        text.text = "";
        patternSpawner.Spawn(PatternCompleted);
    }

    private IEnumerator win()
    {
        text.text = "Win";
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Retry");
    }
    private void OnPatternCompleted()
    {
        StartCoroutine(win());
    }
}
