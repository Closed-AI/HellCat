using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolePattern : Pattern
{
    [SerializeField] private Transform holeTransform;
    [SerializeField] private float forceSpeed;
    [SerializeField] private float sizeSpeed;

    private float timer;
    private CircleCollider2D circleCollider2D;
    private Rigidbody2D playerBody;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        timer = duration * difficult;

        StartCoroutine(PatternRule());
        Destroy(gameObject, timer);
    }

    override protected IEnumerator PatternRule()
    {
        while (timer > 0)
        {
            holeTransform.localScale += Vector3.one * sizeSpeed * Time.deltaTime;
            circleCollider2D.radius += sizeSpeed * Time.deltaTime;

            if (playerBody != null)
            {
                playerBody.AddForce((playerBody.transform.position - transform.position).normalized * forceSpeed, ForceMode2D.Force);
            }

            timer -= Time.deltaTime;

            yield return null;
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
