using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainObjectRule : Rule
{
    [SerializeField] private float rangeX;         // радиус спавна по оси X
    [SerializeField] private float rangeY;         // радиус спавна по оси Y

    override public void Spawn(int id)
    {
        // создание метеорита, назначение начальной позиции
        GameObject obj = Instantiate(arr[id].obj, new Vector2
            (
                Random.Range(-rangeX, rangeX),
                Random.Range(-rangeY, rangeY)
            ), transform.rotation);
    }
}
