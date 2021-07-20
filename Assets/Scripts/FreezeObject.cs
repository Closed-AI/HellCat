using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeObject : MonoBehaviour
{
    [SerializeField] private float reloadingTime;       // время перезарядки заморозки
    [SerializeField] private float freezingTime;        // время действия заморозки
    [SerializeField] private float translationSpeed;    // скорость для анимации притягивания

    private bool available = true;                      // состояние

    private void Start()
    {
        StartCoroutine(Reloading());
    }

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
        Destroy(gameObject);
    }

    private IEnumerator Reloading()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color startColor = spriteRenderer.color;

        spriteRenderer.color = Color.red;

        available = false;
        yield return new WaitForSeconds(reloadingTime);
        available = true;

        spriteRenderer.color = startColor;
    }
}
