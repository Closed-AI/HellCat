using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Animator anim;
    [SerializeField] private float lifeTime;
    private AudioSource audioS;
    private ScoreCounter cash;


    void Start()
    {
        StartCoroutine(LifeDelay());
        audioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        cash = GameObject.Find("GameController").GetComponent<ScoreCounter>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            anim.SetInteger("CoinAnimNumber", 1);
            audioS.Play();
            cash.coins += 1;
            Destroy();
        }       
    }

    IEnumerator LifeDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        anim.SetInteger("CoinAnimNumber", 1);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
