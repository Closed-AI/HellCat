using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Speed of adding score points")]
    public float scoreSpeed;                                       // очков в секунду (базовое значение, которое будет взаимодействовать с множителем счёта)

    [Header("Quiet time before and after the pattern")]
    [SerializeField] private int respiteBefore;
    [SerializeField] private int respiteAfter;

    [Header("The number of points required to start the pattern")]
    public int patternScore;

    private UnityAction PatternCompleted;

    [SerializeField] private GameObject player;
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private PatternSpawner patternSpawner;

    private float scoreForCount;
    private int patternConstScore;

    void Start()
    {
        //player.GetComponent<PlayerController>().OnPlayerDeath += StopScoring;     
        // спавн начинается только при создании нового объекта
        PatternCompleted += OnPatternCompleted;
        obstacleSpawner = Instantiate(obstacleSpawner);
        scoreSpeed = 1f / scoreSpeed;
        StartScoring();
        obstacleSpawner.StartSpawn();
        patternConstScore = patternScore;
    }

    void Update()
    {
        if (player.GetComponent<PlayerController>().alive == false)
            StopScoring();
        scoreForCount = GameObject.Find("GameController").GetComponent<ScoreCounter>().score;
        if (scoreForCount == patternScore)
        {
            patternScore += patternConstScore;
            obstacleSpawner.StopSpawn();
            StartCoroutine(SpawnPattern());
        }
    }

    public IEnumerator SpawnPattern()
    {
        yield return new WaitForSeconds(respiteBefore);
        patternSpawner.Spawn(PatternCompleted);
    }

    public IEnumerator ObstacleActivate()
    {
        yield return new WaitForSeconds(respiteAfter);
        obstacleSpawner.StartSpawn();
    }

    private void OnPatternCompleted()
    {
        GetComponent<ScoreCounter>().PatternsNumber++;
        StartCoroutine(ObstacleActivate());
    }

    private void StartScoring()
    {
        GetComponent<ScoreCounter>().StartScorer();
    }

    private void StopScoring()
    {
        GetComponent<ScoreCounter>().StopScorer();
    }
}
