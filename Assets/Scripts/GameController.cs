using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private UnityAction PatternCompleted;

    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private PatternSpawner  patternSpawner;

    // Start is called before the first frame update
    void Start()
    {
        PatternCompleted += OnPatternCompleted;
        patternSpawner.Spawn(PatternCompleted);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPatternCompleted()
    {
        Debug.Log("PatternCompleted");
    }
}
