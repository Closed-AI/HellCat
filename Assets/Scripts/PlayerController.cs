using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;            // скорость передвижения игрока
    [SerializeField] private FixedJoystick joystick; // ссылка на джойстик
    private Vector2 direction;                       // направление движения игрока
    private Rigidbody2D rb;                          // Rigidbody игрока
    private bool alive;                              // игрок жив    
    private bool isDash;                             // игрок в рывке

    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive && !isDash)
        {
            direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
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

    // при вхождении в триггер (сюда добавлять все условия смерти, замедления и стана)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DamageObject")
        {
            alive = false;
            StartCoroutine(deathAnimation());
        }
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
