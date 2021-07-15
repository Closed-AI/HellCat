using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownObject : MonoBehaviour
{
    [SerializeField] private float speedModifier;

    public float SpeedModifier
    {
        get { return speedModifier; }
    }
}
