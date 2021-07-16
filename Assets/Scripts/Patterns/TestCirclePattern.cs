using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCirclePattern : Pattern
{
    [SerializeField] private Meteor meteor;
    [SerializeField] private float trajectoryLengthY;
    [SerializeField] private float radius;
    [SerializeField] private float cnt;

    private float waveCount;
    // Start is called before the first frame update
    void Start()
    {
        // зависимость от сложности
        waveCount = difficult;
        StartCoroutine(PatternRule());
        Destroy(gameObject, duration);
    }

    override protected IEnumerator PatternRule()
    {
        for (int i = 0; i < waveCount; i++)
        {
            SpawnCircle();

            yield return new WaitForSeconds(duration / waveCount);
        }
    }

    private void SpawnCircle()
    {
        float deltaAngle = 360f / cnt;

        for (int i = 0; i < cnt; i++)
        {
            Vector2 dropPoz = new Vector2(transform.position.x + radius * Mathf.Cos(i * deltaAngle),
                                          transform.position.y + radius * Mathf.Sin(i * deltaAngle));

            Meteor cur = Instantiate(meteor, dropPoz + Vector2.up * trajectoryLengthY, transform.rotation);
            cur.DropPoint = dropPoz;
        }
    }
}
