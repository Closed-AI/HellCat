﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //------------------------------------------------------------------------------------------------//
    //                                     События игрока                                             //
    //------------------------------------------------------------------------------------------------//
    // делегат события смерти
    public delegate void PlayerDeath();                
    public event PlayerDeath OnPlayerDeath;             // событие смерти

    // делегат события заморозки
    public delegate void PlayerFreeze(bool state, FreezeObject stanObject);              
    public event PlayerFreeze OnPlayerFreeze;           // событие заморозки

    // делегат события замедления
    public delegate void PlayerSlowdown(bool state, SlowdownObject slowdownObject);   
    public event PlayerSlowdown OnPlayerSlowdown;       // событие замедления
    //------------------------------------------------------------------------------------------------//
    
    [SerializeField] private float speed;               // скорость передвижения игрока
    [SerializeField] private float dashForce;           // сила рывка
    [SerializeField] private float dashCooldown;        // время отката рывка
    [SerializeField] private FixedJoystick joystick;    // ссылка на джойстик
    private Vector2 direction;                          // направление движения игрока
    private Rigidbody2D rb;                             // Rigidbody игрока

    private float speedModifier = 1f;                   // модификатор скорости (для замедления или ускорения)
    private float dashTime = -2;                         // время, прошедшее после рывка

    private bool alive;                                 // игрок жив    
    private bool isDash;                                // игрок в рывке



    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        rb = GetComponent<Rigidbody2D>();

        OnPlayerDeath    += TakeDamage;
        OnPlayerFreeze   += Freeze;
        OnPlayerSlowdown += Slowdown;
    }

    // Update is called once per frame
    void Update()
    {
        // если игрок жив и не в деше -> перемещение
        if (alive && !isDash)
        {
            direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;
            rb.MovePosition(rb.position + direction * speed * speedModifier * Time.deltaTime);
        }

        if (dashTime > -2)
            dashTime -= Time.deltaTime;
        // цветовая пометка восстановления дэша (потом убрать)
        if (dashTime > -0.2f && dashTime < 0)
            GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f);
        if (dashTime > -1f && dashTime <= -0.2f)
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);


        if (alive && Input.GetKeyDown("space"))
        {
            Dash();
        }
    }

    // при вхождении в триггер (сюда добавлять все условия смерти, замедления и стана)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DamageObject")
            OnPlayerDeath?.Invoke();
        if (collision.tag == "ShakeDamageObject")
            OnPlayerDeath?.Invoke();
        else if (collision.tag == "FreezeObject")
            OnPlayerFreeze?.Invoke(true, collision.GetComponent<FreezeObject>());
        else if (collision.tag == "SlowdownObject")
            OnPlayerSlowdown?.Invoke(true, collision.GetComponent<SlowdownObject>());
    }

    // при выходе из триггера (обработак конца взаимодействия с зоной замеделения, стана и т.д.)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "FreezeObject")
            OnPlayerFreeze?.Invoke(false, collision.GetComponent<FreezeObject>());
        else if (collision.tag == "SlowdownObject")
            OnPlayerSlowdown?.Invoke(false, collision.GetComponent<SlowdownObject>());
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
        rb.MovePosition(rb.position + direction * speed * speedModifier * Time.deltaTime);
    }

    // при смерти запускается эта корутина (добавить анимацию)
    private IEnumerator deathAnimation()
    {
        rb.drag = 100;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Retry");
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

    // механика рывка: вызывающая функция
    public void Dash()
    {
        if (alive && dashTime <= 0)
            StartCoroutine(DashCorouitine());
    }

    // механика рывка: корутина
    private IEnumerator DashCorouitine()
    {
        isDash = true;
        dashTime = dashCooldown;
        direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;
        for (int i = 0; i < 2 ; i++)
        {
            rb.velocity = new Vector2(joystick.Horizontal, joystick.Vertical) * dashForce;
            yield return new WaitForSeconds(0.05f);
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
        }
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        rb.velocity = new Vector2(0, 0);
        isDash = false;
    }
}
