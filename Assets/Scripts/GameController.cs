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

    private AudioSource audioS;
    public AudioClip PatternMusic;
    public AudioClip CoreMusic;
    public AudioClip PatternMusicEnd;
    public AudioClip CoreMusicEnd;

    void Start()
    {
        audioS = GetComponent<AudioSource>();
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
        audioS.Stop();
        audioS.PlayOneShot(CoreMusicEnd);
        yield return new WaitForSeconds(delayBeforePattern);
        audioS.clip = PatternMusic;
        audioS.loop = true;
        audioS.Play();
        patternSpawner.Spawn(PatternCompleted);
    }

    public IEnumerator ObstacleActivate()
    {
        yield return new WaitForSeconds(delayAfterPattern);
        StopAllCoroutines();
        audioS.volume = 1;
        audioS.clip = CoreMusic;
        audioS.loop = true;
        audioS.Play();
        _counter.PatternNumber++;
        _changer.changeDifficult();
        StartScoring();
        obstacleSpawner.StartSpawn();
    }

    private void OnPatternCompleted()
    {
        StartCoroutine(fading_music());
        StartCoroutine(fontain.Spawn(_counter.PatternNumber));
        StartCoroutine(ObstacleActivate());
    }

    private void StartScoring()
    {
        audioS.clip = CoreMusic;
        audioS.loop = true;
        audioS.Play();
        _counter.StartScorer();
    }

    public IEnumerator fading_music()
    {
        while(audioS.volume > 0)
        {
            yield return new WaitForSeconds(0.05f);
            audioS.volume -= 0.01f;
        }
    }

    private void StopScoring()
    {
        _counter.StopScorer();
    }

}
