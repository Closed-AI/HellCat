using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;          // Rigidbody метеорита
    [SerializeField] protected GameObject dropZonePref; // префаб красной зоны падения
    [SerializeField] protected float speed;             // скорость падения

    protected Vector2 dropPoint;                        // точка падения
    protected Vector2 dir;                              // направление ( для перемещения через метод AddForce)
    protected GameObject dropZone;                      // красная зона падения
    protected SpriteRenderer spriteRenderer;            // спрайт красной зоны (для изменения прозрачности)
    protected Color alphaColor;                         // параметры цвета спрайта (см предыдущая строка)
    protected float maxDistance;                        // длина пути
    protected float curDistance;                        // остаточная длина пути
                                                        // две предыдущие переменные нужны для расчёта прозрачности спрайта красной зоны

    // чёртов шарп) это геттер и сеттер для dropZone (строка 13)
    public Vector2 DropPoint
    {
        get { return dropPoint; }
        set
        {
            dropPoint = value;

            if (dropZone == null)
            {
                dropZone = Instantiate(dropZonePref, dropPoint, transform.rotation);
            }
        }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // пока метеорит в полёте, коллайдер зоны падения отключен
        dropZonePref.GetComponent<CapsuleCollider2D>().enabled = false;

        // настройка спрайта зоны падения
        spriteRenderer = dropZone.GetComponent<SpriteRenderer>();
        alphaColor = spriteRenderer.color;
        maxDistance = Vector2.Distance(transform.position, dropPoint);

        // расчёт направления падения
        dir = (dropPoint - (Vector2)transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        // перемещение
        rb.AddForce(dir * speed, ForceMode2D.Force);

        // коррекция прозрачности
        curDistance = Vector2.Distance(transform.position, dropPoint);
        alphaColor.a = 1.25f - (curDistance / maxDistance);
        spriteRenderer.color = alphaColor;

        //  включение коллайдера при падении метеорита
        // (теперь в зоне падения игрок будет умирать)
        //  удаление метеорита после падения
        if (rb.position.y - dropPoint.y <= 0.1f)
        {
            onDrop();
            Destroy(dropZone, 0.2f);
            Destroy(this.gameObject);
        }
    }

    protected virtual void onDrop()
    {
        dropZone.GetComponent<CapsuleCollider2D>().enabled = true;
    }
}