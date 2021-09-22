using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // на уровне достаточно ОДНОГО объекта класса Spawner,
    // правила генерации ВСЕГО записать в массив rules

    // набор правил (каждое правило задаёт... ПРАВИЛА спавна определённых объектов)
    // текущая реализация соответствует паттерну "Фабричный метод"
    // (подставляем конкретный правила в массив Rules)
    [SerializeField] public Rule[] rules;
    // существует базовый класс Rule, от него наследуются конкретные правила
    // (см GlobalMeteorRule и AimMeteorRule)
    // правила спавна задаются путём переопределения виртуального метода Spawn()
    // [ более подробное описание смотри в скрипте Rule ]

    // Start is called before the first frame update
    void Start()
    {
        // конвертация частот в диапазоны
        foreach (var rule in rules)
            rule.ConvertSpawnRates();
    }

    public void StartSpawn()
    {
        // запуск всех спавнеров
        foreach (var rule in rules)
            StartCoroutine(rule.SpawnCoroutine()); // запуск корутины спавнера
    }
    public void StopSpawn()
    {
        StopAllCoroutines();
    }

    // При уничтожении объекта спавнера происходит обратный
    // переход от диапазонов к относительным частотам
    private void OnDestroy()
    {
        foreach (var rule in rules)
            rule.ReversedConvertSpawnRates();
    }
}
