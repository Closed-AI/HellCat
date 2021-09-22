using UnityEngine;

public class BonusRule : Rule
{
    private Transform[] _spawnZones;
    private const float dy = 1f;

    private void Start()
    {
        _spawnZones = new Transform[5];
        for (int i = 0; i < _spawnZones.Length; i++)
        {
            _spawnZones[i] = GameObject.Find("BonusSpawnPoint_" + (i + 1).ToString()).transform;
            System.Console.WriteLine(_spawnZones.Length);
        }
    }

    override public void Spawn(int id)
    {
        // создание бонуса, назначение начальной позиции
        var zone = _spawnZones[Random.Range(0, _spawnZones.Length)];

        Instantiate(arr[id].obj, zone.position + new Vector3(0, dy, 0), zone.rotation);
    }
}
