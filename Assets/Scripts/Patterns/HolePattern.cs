using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolePattern : Pattern
{
    [SerializeField] private Meteor meteor;                                 // префаб метеорита
    [SerializeField] private float meteorCount, meteorDelay;                // количество метеоритов; задержка перед спавном
    [SerializeField] private float trajectoryLengthY;                       // высота спавна метеорита

    [SerializeField] private Transform holeTransform;                       // текстура воронки
    [SerializeField] private float centerForceAmount, normalForceAmount;    // сила притяжения к центру; сила передвижения по перпендикуляру к вектору центра
    [SerializeField] private float deltaCurveAngle;                         // скорость изменения угла
    [SerializeField] private Vector2 curveSize;                             // размер графика
    [SerializeField] private AnimationCurve meteorCurve;                    // график

    private float timer, rstMeteorDelay;
    private float forceMultiplier;
    private float curveAngle;
    private Rigidbody2D playerBody;

    // Start is called before the first frame update
    void Start()
    {
        rstMeteorDelay = meteorDelay/difficult;
        deltaCurveAngle *= difficult;

        timer = duration;

        forceMultiplier = 2f / GetComponent<CapsuleCollider2D>().size.x;

        StartCoroutine(PatternRule());
        Destroy(gameObject, timer);
    }

    override protected IEnumerator PatternRule()
    {
        while (timer > 0)
        {
            if (playerBody != null)
            {
                Vector2 centerDirection = -(playerBody.transform.position - transform.position).normalized;

                float angle = Mathf.Atan2(centerDirection.y, centerDirection.x) + Mathf.PI/2f;

                Vector2 normalDirection = new Vector2(
                    Mathf.Cos(angle), 
                    Mathf.Sin(angle)
                );

                float playerDistance = Vector2.Distance(playerBody.transform.position, transform.position);

                float multiplier = forceMultiplier*playerDistance;

                playerBody.AddForce(centerDirection * centerForceAmount * multiplier, ForceMode2D.Force);
                playerBody.AddForce(normalDirection * normalForceAmount * multiplier, ForceMode2D.Force);
            }
            curveAngle += deltaCurveAngle*Time.deltaTime;

            timer -= Time.deltaTime;
            meteorDelay -= Time.deltaTime;

            if (meteorDelay < 0)
            {
                SpawnCurve(curveAngle);
                meteorDelay = rstMeteorDelay;
            }

            yield return null;
        }
    }
    private void SpawnCurve(float angle)
    {
        float step = 1f/meteorCount;
        float centerX = 0.5f, centerY = 0.5f;

         
        float cosAngle = Mathf.Cos(angle * Mathf.Deg2Rad);
        float sinAngle = Mathf.Sin(angle * Mathf.Deg2Rad);

        for (float x = 0; x < 1f; x += step)
        {
            Vector2 curvePointPosition = new Vector2(
                (x - centerX)* curveSize.x, 
                -(meteorCurve.Evaluate(x) - centerY) * curveSize.y
            );
            Vector2 currentPosition = new Vector2(
                curvePointPosition.x * cosAngle - curvePointPosition.y * sinAngle,
                curvePointPosition.x * sinAngle + curvePointPosition.y * cosAngle
            );

            Meteor newMeteor = Instantiate(meteor, currentPosition + Vector2.up * trajectoryLengthY, meteor.transform.rotation).GetComponent<Meteor>();
            newMeteor.DropPoint = currentPosition;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerBody = collision.attachedRigidbody;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerBody = null;
    }
}
