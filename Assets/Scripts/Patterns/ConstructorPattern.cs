using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WavePoints
{
    public Vector3 pointA;
    public Vector3 pointB;

    public WavePoints(Vector2 pointA, Vector2 pointB)
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
public class ConstructorPattern : Pattern
{
    [SerializeField] protected float meteorCount;
    [SerializeField] protected Meteor meteor;
    [SerializeField] protected float trajectoryLengthY;

    [SerializeField] private WavePart[] waveParts;
    [SerializeField] private bool isRandom;

    protected float waveCount;
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
                    SpawnLine(
                        playerTransform.position + wave.pointA, 
                        playerTransform.position + wave.pointB
                    );
                }

                wavePoints.RemoveAt(waveIndex);

                yield return new WaitForSeconds(duration);
            }

            InitializeWavePointsList();
        }
    }
    virtual protected void InitializeWavePointsList()
    {
        wavePoints = new List<WavePoints[]>();
        for (int i = 0; i < waveParts.Length; i++)
        {
            WavePoints[] points = new WavePoints[waveParts[i].waveTranforms.Length];
            wavePoints.Add(points);

            for (int j = 0; j < waveParts[i].waveTranforms.Length; j++)
            {
                Transform transformA = waveParts[i].waveTranforms[j].Find(pointAString);
                Transform transformB = waveParts[i].waveTranforms[j].Find(pointBString);
                wavePoints[i][j] = new WavePoints(
                    transformA.parent.localPosition + transformA.localPosition,
                    transformB.parent.localPosition + transformB.localPosition
                );
                waveParts[i].waveTranforms[j].gameObject.SetActive(false);
            }
        }
    }
    protected void SpawnLine(Vector2 a, Vector2 b)
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
