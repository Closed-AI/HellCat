using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float _speed;
    public GameObject _player;

    private void Update()
    {
        transform.position = _player.transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
            collision.transform.position = Vector3.Lerp(collision.transform.position, _player.transform.position, Time.deltaTime * _speed);
    }
}
