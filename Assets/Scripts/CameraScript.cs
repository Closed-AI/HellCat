using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject player; // ссылка на GameObject игрока для отслеживания позиции

    // Update is called once per frame
    void Update()
    {
        // слежение за игроком, стоит доработать (добавить плавность)
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
