using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fontain : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _xRange;
    [SerializeField] private float _yRange;

    [SerializeField] private FontainCoin _objectToSpawn;

    private Vector3 pos;

    public IEnumerator Spawn(int patternNumber)
    {
        for (int i = 0; i < patternNumber * 10; i++)
        {
            pos = transform.position;
            pos.x += Random.Range(-_xRange, _xRange);
            pos.y += Random.Range(-_yRange, _yRange);
            FontainCoin coin = Instantiate(_objectToSpawn, transform.position, transform.rotation);
            StartCoroutine(coin.Toss(pos));
            yield return new WaitForSeconds(_duration/(patternNumber * 10));
        }

        yield return null;
    }
}
