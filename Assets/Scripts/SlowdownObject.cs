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
}
