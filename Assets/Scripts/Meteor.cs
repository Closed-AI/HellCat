using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;          // Rigidbody метеорита
    [SerializeField] private GameObject dropZonePref; // префаб красной зоны падения
    [SerializeField] private float speed;             // скорость падения

    private Vector2 dropPoint;                        // точка падения
    private Vector2 dir;                              // направление ( для перемещения через метод AddForce)
    private GameObject dropZone;                      // красная зона падения
    private SpriteRenderer spriteRenderer;            // спрайт красной зоны (для изменения прозрачности)
    private Color alphaColor;                         // параметры цвета спрайта (см предыдущая строка)
    private float maxDistance;                        // длина пути
    private float curDistance;                        // остаточная длина пути
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

        // включение коллайдера при падении метеорита (теперь в зоне падения игрок будет умирать)
        if (Vector2.Distance(rb.position, dropPoint) < 1f)
        {
            dropZone.GetComponent<CapsuleCollider2D>().enabled = true;
        }

        // удаление метеорита после падения
        if (Vector2.Distance(rb.position, dropPoint) < 0.5f)
        {
            Destroy(dropZone);
            Destroy(this.gameObject);
        }
    }
}
