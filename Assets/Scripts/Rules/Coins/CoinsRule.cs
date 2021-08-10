using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsRule : Rule
{
    [SerializeField] private float rangeX;         // радиус спавна по оси X
    [SerializeField] private float rangeY;         // радиус спавна по оси Y
    [SerializeField] private int coinsAmount;      // колличество монет
    [SerializeField] private float radius;         // радиус кольца из монет

    override public void Spawn(int id)
    {
        // создание монет, назначение начальной позиции
        float spawnX = Random.Range(-rangeX + radius, rangeX - radius);
        float spawnY = Random.Range(-rangeY + radius, rangeY - radius);

        for (int i = 0; i < coinsAmount; i++)
        {
            float ang = 360f / coinsAmount * i;
            Vector2 dropPoz = new Vector2(spawnX + radius * Mathf.Sin(ang * Mathf.Deg2Rad),
                                spawnY + radius * Mathf.Cos(ang * Mathf.Deg2Rad));
            GameObject obj = Instantiate(arr[id].obj, dropPoz, transform.rotation);
        }


    }
}
