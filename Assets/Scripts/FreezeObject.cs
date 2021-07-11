﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeObject : MonoBehaviour
{
    [SerializeField] private float reloadingTime;       // время перезарядки заморозки
    [SerializeField] private float freezingTime;        // время действия заморозки
    [SerializeField] private float translationSpeed;    // скорость для анимации притягивания

    private bool available = true;                      // состояние

    public float FreezingTime 
    {
        get { return freezingTime; }
    }
    public float TranslationSpeed 
    {
        get { return translationSpeed; }
    }
    public bool Available 
    {
        get { return available; }
    }

    public void Reload()
    {
        StartCoroutine(Reloading());
    }

    private IEnumerator Reloading()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color rstColor = spriteRenderer.color;

        spriteRenderer.color = Color.red;

        available = false;
        yield return new WaitForSeconds(reloadingTime);
        available = true;

        spriteRenderer.color = rstColor;
    }
}