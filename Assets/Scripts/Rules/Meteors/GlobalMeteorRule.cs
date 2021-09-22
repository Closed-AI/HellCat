using UnityEngine;

public class GlobalMeteorRule : Rule
{
    [SerializeField] private float rangeX;         // радиус спавна по оси X
    [SerializeField] private float rangeY;         // радиус спавна по оси Y
    [SerializeField] private float trajectoryLenX; // МАКСИМАЛЬНАЯ длина тракетории по оси X, метеорит полетит не прямо вниз, а в точук с координатами
                                                   // x = (текущая позиция + случайное число в диапазоне [ -trajectoryLenX; trajectoryLenX ])
                                                   // y = (текущая позиция - trajectoryLenY)
                                                   // (строка 25)
    [SerializeField] private float trajectoryLenY; // длина тракетории по оси Y

    override public void Spawn(int id)
    {
        // создание метеорита, назначение начальной позиции
        GameObject obj = Instantiate(arr[id].obj, new Vector2
            (
                Random.Range(-rangeX, rangeX),
                Random.Range(-rangeY, rangeY) + trajectoryLenY
            ), transform.rotation);

        // создание зоны падения, назначение позиции
        obj.GetComponent<Meteor>().DropPoint = new Vector2
            (
                obj.transform.position.x + Random.Range(-trajectoryLenX, trajectoryLenX),
                obj.transform.position.y - trajectoryLenY
            );
    }
}