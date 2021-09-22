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
    public ScoreCounter _counter;
    private DifficultChanger _changer;

    [Header("Delay before and after the pattern")]
    [SerializeField] private int delayBeforePattern;
    [SerializeField] private int delayAfterPattern;

    private UnityAction PatternCompleted;

    [SerializeField] private GameObject player;
    [SerializeField] public ObstacleSpawner obstacleSpawner;
    [SerializeField] private PatternSpawner patternSpawner;
    [SerializeField] private Fontain fontain;

    public float nextPatternScore;
    public float patternScoreAdd = 100;
    public float addForAdd = 50;

    void Start()
    {
        _counter = GetComponent<ScoreCounter>();
        _changer = GetComponent<DifficultChanger>();
        _counter.score = 0;
        nextPatternScore = patternScoreAdd;

        PatternCompleted += OnPatternCompleted;
        scoreSpeed = 0.1f;
        _changer.changeDifficult();
        StartScoring();
        obstacleSpawner.StartSpawn();
    }

    void Update()
    {
        if (player.GetComponent<PlayerController>().alive == false)
            StopScoring();
        if (_counter.score == nextPatternScore)
        {
            StopScoring();

            // destroy all dangers

            patternScoreAdd += addForAdd;
            nextPatternScore += patternScoreAdd;

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
        _counter.PatternNumber++;
        _changer.changeDifficult();
        StartScoring();
        obstacleSpawner.StartSpawn();
    }

    private void OnPatternCompleted()
    {
        StartCoroutine(fontain.Spawn(_counter.PatternNumber));
        StartCoroutine(ObstacleActivate());
    }

    private void StartScoring()
    {
        _counter.StartScorer();
    }

    private void StopScoring()
    {
        _counter.StopScorer();
    }

}
