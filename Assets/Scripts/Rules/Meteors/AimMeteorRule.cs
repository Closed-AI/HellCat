using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMeteorRule : Rule
{
    private GameObject player;                     // ссылка на GameObject игрока для отслеживания позиции
    [SerializeField] private float offsetX;        // смещение точки падения метеорита по оси X
    [SerializeField] private float offsetY;        // смещение точки падения метеорита по оси Y
    [SerializeField] private float trajectoryLenX; // МАКСИМАЛЬНАЯ длина тракетории по оси X, 
                                                   // в данном случае начальная позиция метеорита по оси X
                                                   // будет смещена относительно положения игока на случайное значение 
                                                   // в промежутке [ -trajectoryLenX; trajectoryLenX ] для задания угла падения
                                                   // (строка 26)
    [SerializeField] private float trajectoryLenY; // длина тракетории по оси Y
    override public void Spawn(int id)
    {
        // поиск игрока (если вынести этот код в функцию Start - вылезает ошибка
        // скорее всего при появлении этого оъекта игрок ещё не создан на сцене)
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        // создание метеорита, назначение начальной позиции
        GameObject obj = Instantiate(arr[id].obj,
                                            new Vector2
                                            (
                                                player.transform.position.x + Random.Range(-trajectoryLenX, trajectoryLenX),
                                                player.transform.position.y + trajectoryLenY
                                            ),
                                            transform.rotation);

        // создание зоны падения, назначение позиции
        obj.GetComponent<Meteor>().DropPoint = new Vector2
            (
                player.transform.position.x + Random.Range(-offsetX, offsetX),
                player.transform.position.y + Random.Range(-offsetY, offsetY)
            );
    }
}
