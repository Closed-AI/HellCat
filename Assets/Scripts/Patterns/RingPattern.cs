using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPattern : Pattern
{
    private GameObject player;
    private float speed;
    [SerializeField] private float size;                                                       // Размер кольца
    [SerializeField] private float ArenaLeftPos, ArenaRightPos, ArenaUpPose, ArenaDownPos;     // Крайние точки перемещения кольца
    private float targetX = 0, targetY = 0;                                                    // Точки, куда сдвигаются
    private Vector3 posEnd;

    void Start()
    {
        speed = difficult;
        Destroy(gameObject, duration);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (transform.localScale.x > size)
            {
                transform.localScale -= new Vector3(speed * 0.5f, speed * 0.5f, 0) * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 1);
            }
            else
            {
                if (transform.position.x < targetX + 0.5f && transform.position.x > targetX - 0.5f &&
                    transform.position.y < targetY + 0.5f && transform.position.y > targetY - 0.5f)
                {
                    targetX = Random.Range(ArenaLeftPos, ArenaRightPos);
                    targetY = Random.Range(ArenaDownPos, ArenaUpPose);
                    posEnd = new Vector3(targetX, targetY, transform.position.z);
                }
                else
                    transform.position = Vector3.MoveTowards(transform.position, posEnd, speed * 0.005f);
            }
        }

    }
}
