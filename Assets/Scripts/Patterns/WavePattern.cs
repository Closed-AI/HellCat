using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WavePoints
{
    public Transform pointA;
    public Transform pointB;

    public WavePoints(Transform pointA, Transform pointB)
    {
        this.pointA = pointA;
        this.pointB = pointB;
    }
}
[Serializable]
public class WavePart
{
    public Transform[] waveTranforms;
}
public class WavePattern : Pattern
{
    [SerializeField] private WavePart[] waveParts;
    [SerializeField] private Meteor meteor;
    [SerializeField] private float trajectoryLengthY;
    [SerializeField] private float meteorCount;
    [SerializeField] private bool isRandom;

    private float waveCount;
    private Transform playerTransform;
    private List<WavePoints[]> wavePoints;

    private const string pointAString = "pointA";
    private const string pointBString = "pointB";
    private const string playerString = "Player";

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag(playerString).transform;

        InitializeWavePointsList();

        waveCount = difficult;
        StartCoroutine(PatternRule());
        Destroy(gameObject, duration*waveCount);
    }

    override protected IEnumerator PatternRule()
    {
        for (int i = 0; i < waveCount; i++)
        {
            while(wavePoints.Count > 0)
            {
                int waveIndex = 0;

                if (isRandom)
                    waveIndex = UnityEngine.Random.Range(0, wavePoints.Count);

                foreach (WavePoints wave in wavePoints[waveIndex])
                {
                    SpawnLine(playerTransform.position + wave.pointA.localPosition + wave.pointA.parent.localPosition, playerTransform.position + wave.pointB.localPosition + wave.pointB.parent.localPosition);
                }

                wavePoints.RemoveAt(waveIndex);

                yield return new WaitForSeconds(duration / waveCount);
            }

            InitializeWavePointsList();
            
          //  yield return new WaitForSeconds(duration / waveCount);
        }
    }
    private void InitializeWavePointsList()
    {
        wavePoints = new List<WavePoints[]>();
        for (int i = 0; i < waveParts.Length; i++)
        {
            WavePoints[] points = new WavePoints[waveParts[i].waveTranforms.Length];
            wavePoints.Add(points);

            for (int j = 0; j < waveParts[i].waveTranforms.Length; j++)
            {
                wavePoints[i][j] = new WavePoints(
                    waveParts[i].waveTranforms[j].Find(pointAString),
                    waveParts[i].waveTranforms[j].Find(pointBString)
                );
                waveParts[i].waveTranforms[j].gameObject.SetActive(false);
            }
        }
    }
    private void SpawnLine(Vector2 a, Vector2 b)
    {
        Vector2 currentPosition = a;
        Vector2 direction = (b - a).normalized;

        float deltaPosition = (b - a).magnitude / meteorCount;

        for(int i = 0; i < meteorCount; i++)
        {
            Meteor newMeteor = Instantiate(meteor, currentPosition + Vector2.up * trajectoryLengthY, meteor.transform.rotation).GetComponent<Meteor>();
            newMeteor.DropPoint = currentPosition;

            currentPosition += deltaPosition * direction;
        }    
    }
}
