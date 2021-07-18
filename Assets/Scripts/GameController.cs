using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private UnityAction PatternCompleted;

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
        patternSpawner.Spawn(PatternCompleted);
    }

    private void OnPatternCompleted()
    {
        Debug.Log("PatternCompleted");
    }
}
