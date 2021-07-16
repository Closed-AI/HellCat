﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float shaking = 0;
    [SerializeField] private float shakeDelay;          // длительность тряски
    [SerializeField] private float offsetX, offsetY;    // сила тряски по x и y
    [SerializeField] private GameObject player;         // ссылка на GameObject игрока для отслеживания позиции
    private Vector3 posEnd;                             // позиция к которой будет стремиться камера
    [SerializeField] private float smooth;              // Значение, используемое для интерполяции между a и b
                                                        // если значение smooth - 1 - камера переместится к персонажу на один кадр
                                                        // если значение smooth - 0.125 - камера переместится к персонажу за 8 кадров и т.д.


    void FixedUpdate()
    {
        // перемещение камеры
        posEnd = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, posEnd, smooth);
        // тряска камеры
        if (shaking > 0)
        {
            transform.position = new Vector3(transform.position.x + Random.Range(-offsetX, offsetX), transform.position.y + Random.Range(-offsetY, offsetY), transform.position.z);
            shaking -= Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "ShakeDamageObject")
            shaking = shakeDelay;
    }
}
