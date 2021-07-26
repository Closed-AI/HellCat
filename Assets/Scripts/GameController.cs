using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private UnityAction PatternCompleted;

    [SerializeField] private GameObject player;
    [SerializeField] private Text text;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private PatternSpawner  patternSpawner;
    [SerializeField] private float scoreSpeed;                // очков в секунду (базовое значение, которое будет взаимодействовать с множителем счёта)
    [SerializeField] private int maxScore;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<PlayerController>().OnPlayerDeath += StopScoring;
        PatternCompleted += OnPatternCompleted;  // удалить эту строку, когда сделаешь бесконечный режим
        // спавн начинается только при создании нового объекта
        obstacleSpawner = Instantiate(obstacleSpawner);
        score = 0;
        scoreSpeed = 1f / scoreSpeed;
        StartCoroutine(addScore());
        text.text = "";
    }

    private IEnumerator addScore()
    {
        // переписать код прогресс бара, для начала сделать паттерны каждые N очков
        // ( под это дело, которое N можно сделать новую переменную для инспектора
        //  что нибудь вроде patternScore )
        while (score < maxScore)
        {
            score++;
            progressBar.GetComponent<ProgressBarController>().SetVal((float)score / maxScore);
            yield return new WaitForSeconds(scoreSpeed);
        }

        obstacleSpawner.StopSpawn();
        text.text = "Danger";
        yield return new WaitForSeconds(1.5f); // задержка между концом волны и началом паттерна
        text.text = "";
        patternSpawner.Spawn(PatternCompleted);
    }

    // удалить, когда сделаешь бесконечный режим
    private IEnumerator Win()
    {
        if (text != null)
            text.text = "Win";
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Retry");
    }

    // удалить, когда сделаешь бесконечный режим
    private void OnPatternCompleted()
    {
        StartCoroutine(Win());
    }

    private void StopScoring()
    {
        // тут прописать остановку корутины в классе - счётчике очков
    }
}
