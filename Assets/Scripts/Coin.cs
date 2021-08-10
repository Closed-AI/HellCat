using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Animator anim;
    [SerializeField] private float lifeTime;
    private AudioSource audioS;


    void Start()
    {
        StartCoroutine(LifeDelay());
        audioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            anim.SetInteger("CoinAnimNumber", 1);
            audioS.Play();
            GameObject.Find("GameController").GetComponent<ScoreCounter>().coins += 1;
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
