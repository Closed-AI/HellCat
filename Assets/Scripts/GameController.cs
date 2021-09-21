using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class BONUS
{
    public const int SHIELD       = 0;
    public const int DASH         = 1;
    public const int MAGNET       = 2;
}

public class GameController : MonoBehaviour
{
    [Header("Speed of adding score points")]
    public float scoreSpeed;
    public ScoreCounter counter;

    [Header("Delay before and after the pattern")]
    [SerializeField] private int delayBeforePattern;
    [SerializeField] private int delayAfterPattern;

    [Header("The number of points required to start the pattern")]
    public float patternScore;

    private UnityAction PatternCompleted;

    [SerializeField] private GameObject player;
    [SerializeField] public  ObstacleSpawner obstacleSpawner;
    [SerializeField] private PatternSpawner patternSpawner;

    private float scoreForCount;
    private float patternConstScore;

    private readonly int[] _levelScores = { 100, 250, 450, 700, 1000, 1500, 2250, 3500, 5000 };

    void Start()
    {
        counter = GetComponent<ScoreCounter>();

        PatternCompleted += OnPatternCompleted;
        scoreSpeed = 0.1f;
        StartScoring();
        obstacleSpawner.StartSpawn();
        patternConstScore = patternScore;
    }

    void Update()
    {
        if (player.GetComponent<PlayerController>().alive == false)
            StopScoring();
        scoreForCount = counter.score;
        if (scoreForCount == patternScore)
        {
            StopScoring();
            // destroy all dangers
            //if (patternScore < _levelScores[_levelScores.Length - 1])
            //    for (int i = 0; i < _levelScores.Length; i++)
            //    {
            //        if (patternScore <)
            //    }
            //else
                patternScore += 5000;
            obstacleSpawner.StopSpawn();
            StartCoroutine(SpawnPattern());
        }
    }

    public IEnumerator SpawnPattern()
    {
        yield return new WaitForSeconds(delayBeforePattern);
        patternSpawner.Spawn(PatternCompleted);
    }

    public IEnumerator ObstacleActivate()
    {
        yield return new WaitForSeconds(delayAfterPattern);
        obstacleSpawner.StartSpawn();
    }

    private void OnPatternCompleted()
    {
        StartScoring();
        counter.PatternsNumber++;
        StartCoroutine(ObstacleActivate());
    }

    private void StartScoring()
    {
        counter.StartScorer();
    }

    private void StopScoring()
    {
        counter.StopScorer();
    }
}
