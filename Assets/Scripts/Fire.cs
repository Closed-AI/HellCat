using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private float TimeDelay;          // задержка перед исчезновением огненой зоны

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeDelay);
    }
}
