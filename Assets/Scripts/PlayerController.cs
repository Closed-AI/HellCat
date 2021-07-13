﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // делегат события смерти
    public delegate void PlayerDeath();                
    public event PlayerDeath OnPlayerDeath;             // событие смерти

    // делегат события заморозки
    public delegate void PlayerFreeze(bool state, FreezeObject stanObject);              
    public event PlayerFreeze OnPlayerFreeze;           // событие заморозки

    // делегат события замедления
    public delegate void PlayerSlowdown(bool state, SlowdownObject slowdownObject);   
    public event PlayerSlowdown OnPlayerSlowdown;       // событие замедления

    [SerializeField] private float speed;               // скорость передвижения игрока
    [SerializeField] private FixedJoystick joystick;    // ссылка на джойстик
    private Vector2 direction;                          // направление движения игрока
    private Rigidbody2D rb;                             // Rigidbody игрока

    private float speedModifier = 1f;                   // модификатор скорости (для замедления или ускорения)

    private bool alive;                                 // игрок жив    
    private bool isDash;                                // игрок в рывке

    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        rb = GetComponent<Rigidbody2D>();

        OnPlayerDeath += TakeDamage;
        OnPlayerFreeze += Freeze;
        OnPlayerSlowdown += Slowdown;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive && !isDash)
        {
            direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;
            
            rb.MovePosition(rb.position + direction.normalized * speed * speedModifier * Time.deltaTime);
        }
    }

    // при смерти запускается эта корутина (добавить анимацию)
    private IEnumerator deathAnimation()
    {
        transform.rotation = Quaternion.Euler(0, 0, -90);
        Vector3 newPos = transform.position;
        transform.position = newPos;

        yield return new WaitForSeconds(1.5f);
        Application.LoadLevel("Retry");
    }

    // выполняется в OnPlayerDeath
    private void TakeDamage()
    {
        alive = false;
        StartCoroutine(deathAnimation());
    }
    // выполняется в OnPlayerFreeze
    private void Freeze(bool state, FreezeObject freezeObject)
    {
        bool hasFreezing = state && freezeObject.Available;

        speedModifier = hasFreezing ? 0 : 1;

        if (hasFreezing)
        {
            StartCoroutine(Freezing(freezeObject));
        }
    }
    // выполняется в OnPlayerSlowdown
    private void Slowdown(bool state, SlowdownObject slowdownObject)
    {
        speedModifier = state ? slowdownObject.SpeedModifier : 1;
    }
    // корутина для анимирования заморозки
    private IEnumerator Freezing(FreezeObject freezeObject)
    {
        float translationSpeed = freezeObject.TranslationSpeed;
        float timer = freezeObject.FreezingTime;

        while(timer > 0)
        {
            timer -= Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, freezeObject.transform.position, Time.deltaTime* translationSpeed);

            yield return null;
        }

        OnPlayerFreeze?.Invoke(false, freezeObject);

        freezeObject.Reload();
    }
    // при вхождении в триггер (сюда добавлять все условия смерти, замедления и стана)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DamageObject")
            OnPlayerDeath?.Invoke();
        else if (collision.tag == "FreezeObject")
            OnPlayerFreeze?.Invoke(true, collision.GetComponent<FreezeObject>());
        else if (collision.tag == "SlowdownObject")
            OnPlayerSlowdown?.Invoke(true, collision.GetComponent<SlowdownObject>());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "FreezeObject")
            OnPlayerFreeze?.Invoke(false, collision.GetComponent<FreezeObject>());
        else if (collision.tag == "SlowdownObject")
            OnPlayerSlowdown?.Invoke(false, collision.GetComponent<SlowdownObject>());
    }

    // механика рывка (нуждается в переработке)
    public void Dash()
    {
        StartCoroutine(DashCorouitine());
    }

    // механика рывка ( The kostil. Part 2)
    private IEnumerator DashCorouitine()
    {
        isDash = true;
        direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;

        for (int i = 0; i < 2 ; i++)
        {
            rb.MovePosition(rb.position + direction * 15 * speed * Time.deltaTime);
            yield return new WaitForSeconds(0.05f);
        }
        isDash = false;
    }
}
