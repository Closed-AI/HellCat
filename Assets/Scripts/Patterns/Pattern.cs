using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pattern : MonoBehaviour
{
    [SerializeField] protected float difficult; // сложность - адаптивный параметр, от которого будут
                                                // зависеть все остальные параметры конкретного паттерна
                                                // варьирует в диапазоне [ 1; 100 ], от этого параметра 
                                                // зависят все свойства конкретного паттерна ( их необходимо
                                                // настраивать в каждой конкретной реализации в методе Start())
    [SerializeField] protected float duration;  // длительность работы паттерна

    protected UnityAction PatternCompleted;

    private void Start()
    {

    }

    virtual protected IEnumerator PatternRule()
    {
        yield return null;
    }

    public void AddPatternEndListener(UnityAction action)
    {
        PatternCompleted = action;
    }

    private void OnDestroy()
    {
        // если игрок умерает, не пройдя паттерн,
        // то объект GameController уничтожается раньше,
        // чем паттерн и выкидывает ошибку
        try
        {
            PatternCompleted.Invoke();
        }
        catch
        {

        }
    }
}
