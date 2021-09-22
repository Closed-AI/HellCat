using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class RULE_ENUM
{
    static public int BONUS  = 0;
    static public int AIM    = 1;
    static public int GLOBAL = 2;
    static public int COINS  = 3;
    static public int TRAP   = 4;
}

public class DifficultChanger : MonoBehaviour
{
    private GameController _controller;
    private ObstacleSpawner _spawner;

    public void changeDifficult()
    {
        _controller = GetComponent<GameController>();
        _spawner = GetComponentInChildren<ObstacleSpawner>();

        int patternNum = _controller._counter.PatternNumber;

        _spawner.rules[RULE_ENUM.BONUS ].absSpawnRate = 15f + (patternNum > 1 ? 0f : 99999f);
        _spawner.rules[RULE_ENUM.AIM   ].absSpawnRate = Mathf.Max(3f - (2.7f * patternNum / 20f), 0.1f);
        _spawner.rules[RULE_ENUM.GLOBAL].absSpawnRate = 0.2f / patternNum;
        _spawner.rules[RULE_ENUM.COINS ].absSpawnRate = 2f + Mathf.Max(10f - patternNum, 0f);
        _spawner.rules[RULE_ENUM.TRAP  ].absSpawnRate = 6f / patternNum;
    }
}