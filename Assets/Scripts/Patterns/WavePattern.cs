using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePattern : Pattern
{
    [SerializeField] private float startDelay;                       // задержка перед спавном
    [SerializeField] private Meteor meteor;                          // префаб метеорита
    [SerializeField] private float meteorSpeed;                      // скорость падения метеоритов
    [SerializeField] private float meteorCount;                      // количество метеоритов в линию
    [SerializeField] private float trajectoryLengthY;                // высота спавна метеорита

    [SerializeField] private float[] angles;                         // количество углов - количество волн; значение угла - поворот этой волны
    [SerializeField] private float wavesSpawnSpeed, wavesDistance;   // скорость генерации волны, растояние между волнами
    [SerializeField] private Vector2 minBorderSpawn, maxBorderSpawn; // границы спавна

    private Vector2 currentDirection;
    private Vector2 currentPositionA, currentPositionB;
    private Vector2 currentWaveCenter, endPosition;

    void Start()
    {
        InitializeNewDirection(angles[0]);
        
        StartCoroutine(PatternRule());
    }
    override protected IEnumerator PatternRule()
    {
        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < angles.Length; i++)
        {
            InitializeNewDirection(angles[i]);

            while (Vector2.Distance(currentWaveCenter, endPosition) > wavesDistance)
            {
                SpawnLine(currentPositionA, currentPositionB);

                Vector2 deltaPosition = currentDirection * wavesDistance;

                currentPositionA += deltaPosition;
                currentPositionB += deltaPosition;
                currentWaveCenter += deltaPosition;

                // зависимость от сложности
                yield return new WaitForSeconds(wavesSpawnSpeed/difficult);
            }
        }

        if (transform.parent != null && transform.parent.childCount < 2)
            Destroy(this.transform.parent.gameObject);
        else Destroy(this.gameObject);
    }
    private void InitializeNewDirection(float angle)
    {
        float waveLength = Vector2.Distance(maxBorderSpawn, minBorderSpawn);
        float radius = waveLength / 2f;

        currentWaveCenter = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
            Mathf.Sin(angle * Mathf.Deg2Rad) * radius
        );
        endPosition = -currentWaveCenter;

        // Текущее направление
        currentDirection = -currentWaveCenter.normalized;

        // Текущая позиция
        currentPositionA = new Vector2(
            Mathf.Cos((90 + angle) * Mathf.Deg2Rad) * radius,
            Mathf.Sin((90 + angle) * Mathf.Deg2Rad) * radius
        );

        currentPositionB = -currentPositionA;

        // Позиция центра
        currentPositionA += currentWaveCenter;
        currentPositionB += currentWaveCenter;
    }
    private void SpawnLine(Vector2 a, Vector2 b)
    {
        Vector2 currentPosition = a;
        Vector2 direction = (b - a).normalized;

        float deltaPosition = (b - a).magnitude / meteorCount;

        for (int i = 0; i < meteorCount; i++)
        {
            Meteor newMeteor = Instantiate(meteor, currentPosition + Vector2.up * trajectoryLengthY, meteor.transform.rotation).GetComponent<Meteor>();
            newMeteor.Speed = meteorSpeed;
            newMeteor.DropPoint = currentPosition;

            currentPosition += deltaPosition * direction;
        }
    }
}
