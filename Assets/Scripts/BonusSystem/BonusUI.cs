using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: if i find the time

public class BonusUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] bonusSprites;
    private PlayerController _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
}
