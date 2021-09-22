using System.Collections;
using UnityEngine;

public class DashBonus : MonoBehaviour
{
    [SerializeField] private GameObject _playerObj;
    private PlayerController _player;

    [SerializeField] private GameObject _daugterObj;

    [SerializeField] private float dashForce;           // сила рывка
    [SerializeField] private float dashDuration;        // длительность рвыка
    public float dashCooldown;                          // время отката рывка
    public float dashTime;                              // время, прошедшее после рывка
    public bool isDash;                                 // игрок в рывке

    // Start is called before the first frame update
    void Start()
    {
        _player = _playerObj.GetComponent<PlayerController>();

        isDash = false;
        dashTime = dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (dashTime < dashCooldown)
            dashTime += Time.deltaTime;
    }

    // механика рывка: вызывающая функция
    public void Dash()
    {
        if (dashTime >= dashCooldown)
            StartCoroutine(DashCorouitine());
    }

    public IEnumerator DashCorouitine()
    {
        isDash = true;
        dashTime = 0;

        _player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);

        _player.rb.velocity = new Vector2(_player.joystick.Horizontal, _player.joystick.Vertical).normalized * dashForce;

        yield return new WaitForSeconds(dashDuration);

        _player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        _player.rb.velocity = new Vector2(0, 0);
        isDash = false;
    }

    void OnEnable()
    {
        _daugterObj.SetActive(true);
    }

    void OnDisable()
    {
        _daugterObj.SetActive(false);
    }
}