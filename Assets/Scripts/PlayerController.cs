using System.Collections;
using UnityEngine;

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
    [SerializeField] private float dashDuration;        // длительность рвыка
    [SerializeField] public FixedJoystick joystick;    // ссылка на джойстик
    public float dashCooldown;                          // время отката рывка
    private Vector2 direction;                          // направление движения игрока
    public Rigidbody2D rb;                             // Rigidbody игрока

    private float speedModifier = 1f;                   // модификатор скорости (для замедления или ускорения)
    public float dashTime;                              // время, прошедшее после рывка

    public bool alive;                                  // игрок жив    
    public bool isDash;                                 // игрок в рывке
                                                        
    public GameObject FinalScreen;                      // окно финального счёта

    [SerializeField] private BonusesOnPlayer bonusPref; // бонусы
    public float[]      bonusTime;                     // длительность бонусов

    // Start is called before the first frame update
    void Start()
    {
        bonusTime = new float[3];

        for (int i = 0; i < bonusTime.Length; i++) bonusTime[i] = 0;

        dashTime = dashCooldown;
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
        if (alive && !isDash &&
            ((bonusPref.bonuses[BONUS.DASH].activeSelf == true && !bonusPref.bonuses[BONUS.DASH].GetComponent<DashBonus>().isDash) ||
              bonusPref.bonuses[BONUS.DASH].activeSelf == false))
        {
            direction = joystick.Direction;
            rb.MovePosition(rb.position + direction * speed * speedModifier * Time.deltaTime);

            if (Input.GetKeyDown("space"))
                Dash();
        }
        
        if (dashTime < dashCooldown)
            dashTime += Time.deltaTime;

        for (int i = 0; i < 3; i++)
        {
            bonusTime[i] -= Time.deltaTime;
            if (bonusTime[i] <= 0) bonusPref.bonuses[i].SetActive(false);
        }
    }

    // при нахождении в триггере (сюда добавлять все условия смерти, замедления и стана)
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isDash)
        {
            if (collision.tag == "DamageObject")
                OnPlayerDeath?.Invoke();
            else if (collision.tag == "ShakeDamageObject")
                OnPlayerDeath?.Invoke();
            else if (collision.tag == "FreezeObject")
                OnPlayerFreeze?.Invoke(true, collision.GetComponent<FreezeObject>());
            else if (collision.tag == "SlowdownObject")
                OnPlayerSlowdown?.Invoke(true, collision.GetComponent<SlowdownObject>());
        }
        
        if (collision.tag == "Bonus")
        {
            switch (collision.GetComponent<BonusObject>().BonusType)
            {
                case BONUS.SHIELD:
                    bonusPref.bonuses[BONUS.SHIELD].SetActive(true);
                    bonusTime[BONUS.SHIELD] = SaveSystem.Instance.Progress[BONUS.SHIELD] + 5;
                    break;
                case BONUS.DASH:
                    bonusPref.bonuses[BONUS.DASH].SetActive(true);
                    bonusTime[BONUS.DASH] = SaveSystem.Instance.Progress[BONUS.DASH] + 5;
                    break;
                case BONUS.MAGNET:
                    bonusPref.bonuses[BONUS.MAGNET].SetActive(true);
                    bonusTime[BONUS.MAGNET] = SaveSystem.Instance.Progress[BONUS.MAGNET] + 5;
                    break;
            }

            Destroy(collision.gameObject);
        }
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
        if (bonusPref.bonuses[BONUS.SHIELD].activeSelf == true)
        {
            bonusPref.bonuses[BONUS.SHIELD].SetActive(false);
            bonusTime[BONUS.SHIELD] = 0.2f;
        }


        if (bonusTime[BONUS.SHIELD]>0) return;

        for (int i = 0; i < 3; i++)
            bonusPref.bonuses[i].SetActive(false);

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
        FinalScreen.SetActive(true);
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

            yield return Time.deltaTime;
        }

        OnPlayerFreeze?.Invoke(false, freezeObject);

        if (freezeObject != null)
            freezeObject.Reload();
    }

    // механика рывка: вызывающая функция
    public void Dash()
    {
        if (alive)
        {
            if (dashTime >= dashCooldown)
                StartCoroutine(DashCorouitine());
            else if (bonusPref.bonuses[BONUS.DASH].activeSelf == true)
                bonusPref.bonuses[BONUS.DASH].GetComponent<DashBonus>().Dash();
        }
    }

    // механика рывка: корутина
    public IEnumerator DashCorouitine()
    {
        isDash = true;
        dashTime = 0;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);

        rb.velocity = new Vector2( joystick.Horizontal, joystick.Vertical).normalized * dashForce;

        yield return new WaitForSeconds(dashDuration);

        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        rb.velocity = new Vector2(0, 0);
        isDash = false;
    }
}