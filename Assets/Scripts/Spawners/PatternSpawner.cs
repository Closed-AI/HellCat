using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PatternSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] patterns;
    
    public void Spawn(UnityAction action)
    {
        int index = UnityEngine.Random.Range(0, patterns.Length);

        Instantiate(patterns[index]).GetComponent<Pattern>().AddPatternEndListener(action);
    }
}
