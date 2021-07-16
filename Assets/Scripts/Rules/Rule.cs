using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// самопальная структурка) для объединения игрового объекта и частоты (ОТНОСИТЕЛЬНОЙ) его появления
[Serializable]
public class Map
{
    public GameObject obj;
    public float spawnRate;
}

public class Rule : MonoBehaviour
{
    [SerializeField] protected float startDelay;   // задержка перед началом спавна (на данном этапе используется для правила AimMeteorRule)
                                                   // чтобы самонаводящиеся метеориты появлялись не сразу и дали игроку схватиться за джойстик

    [SerializeField] protected float absSpawnRate; // общая частота спавна объектов из этого правила

    [SerializeField] protected Map[] arr;          // массив объектов, структур (строка 8) содержащих объект для спавна и его ОТНОСИТЕЛЬНУЮ частоту
                                                   // [ относительная частота - число в пределах от 0 до 1, показывающая, какую ]
                                                   // [ часть от общего числа создаваемых объектов будет занимать текущий       ]
                                                   // [ ВАЖНО!!! сумма относительных частот должна равняться единице!!!         ]
                                                   // ВАЖНО!!! поведение объекта определяется самим объектом, спавнер должен лишь задавать 
                                                   // начальные параметры (место спавна и т.д.)

    // кешированные поля для случайного выбора объекта из списка arr (строка 21)
    private int objectIndex;                       // индекс текущего объекта для спавна
    private float randVal;                         // значение генератора случайных чисел для определения objectIndex (строка 29)

    // виртуальный метод Spawn(), именно он будет вызываться в корутине SpawnCorutine() (строка 44) и определять правила спавна
    // этот метод переопределяется в каждой конкретной реализации правила (классе, наследуемом от Rule)
    // [ смотри примеры: скрипты GlobalMeteorRule и AimMeteorRule ]
    // параметр id отвечает за то, какой именно объект из списка arr (строка 21) будет создан в текущей итерации
    virtual public void Spawn(int id)            
    {
        
    }

    // корутина, запускающая процесс спавна, оперирует переменными
    // startDelay   - задержка, перед началом спавна (может быть нулевой)
    // absSpawnRate - общая частота спавна объектов
    public IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // выбор объекта для спавна из списка arr (строка 21)
            randVal = UnityEngine.Random.Range(0f, 1f);
            for (objectIndex = 0; objectIndex < arr.Length; objectIndex++)
                if (randVal <= arr[objectIndex].spawnRate)
                    break;

            // сам спавн
            Spawn(objectIndex);

            yield return new WaitForSeconds(absSpawnRate);
        }
    }

    // конвертация частот в диапазоны 
    // ( в редакторе юнити вводятся относительные частоты, например: 0.7 0.2 0.1
    //   а код работает с диапазонами, а именно 0.7 0.9 1.0
    //   или [ 0; 0.7 ), [ 0.7; 0.9 ), [ 0.9; 1 ] (в функции SpawnCoroutine() (строка 44) 
    //   происходит проверка случайного числа на пренадлежность к одному из этих диапазонов )
    public void ConvertSpawnRates()
    {
        for (int i = 1; i < arr.Length; i++)
            arr[i].spawnRate = arr[i].spawnRate + arr[i - 1].spawnRate;
    }

    // метод, обратный ConvertSpawnRates() (строка 68)
    // конвертация диапазонов в относительные частоты
    public void ReversedConvertSpawnRates()
    {
        for (int i = arr.Length - 1; i > 0; i--)
            arr[i].spawnRate = arr[i].spawnRate - arr[i - 1].spawnRate;
    }
}