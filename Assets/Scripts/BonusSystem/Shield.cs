using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _player.transform.position, 1);
    }
}
