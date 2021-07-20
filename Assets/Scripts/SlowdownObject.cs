using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownObject : MonoBehaviour
{
    [SerializeField] private float speedModifier;
    [SerializeField] private float lifeTime;

    public void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public float SpeedModifier
    {
        get { return speedModifier; }
    }

    private void OnDestroy()
    {
        Collider2D collider2D = GetComponent<Collider2D>();

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale/2f, 0);

        for (int i = 0; i < colliders.Length; i++)
            if(colliders[i].tag == "Player")
                colliders[i].BroadcastMessage("OnTriggerExit2D", collider2D);
    }
}
