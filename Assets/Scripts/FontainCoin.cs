using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontainCoin : MonoBehaviour
{
    Animator anim;
    private AudioSource audioS;
    private ScoreCounter cash;

    [SerializeField] private float timeBeforeActivate;
    private Vector3 startPos;
    private Vector3 buff;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos;

        audioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        cash = GameObject.Find("GameController").GetComponent<ScoreCounter>();
    }


    public IEnumerator Toss(Vector3 endPos)
    {
        float time = 0;
        float xSpeed = endPos.x - startPos.x;
        float ySpeed = endPos.y - startPos.y + 9.8f * timeBeforeActivate; // if error try reverse END and START sign's

        while (time < timeBeforeActivate)
        {
            ySpeed -= 9.8f * 2f * Time.deltaTime;

            buff.x += xSpeed * Time.deltaTime;
            buff.y += ySpeed * Time.deltaTime;// startPos.y + (ySpeed - 9.8f * time);

            transform.position = buff;

            time += Time.deltaTime;
            yield return Time.deltaTime;
        }
        
        GetComponent<CircleCollider2D>().enabled = true;
        Debug.Log("Was enabled");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            anim.SetInteger("CoinAnimNumber", 1);
            audioS.Play();
            cash.coins += 1;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}