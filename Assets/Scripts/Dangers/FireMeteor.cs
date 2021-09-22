using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMeteor : Meteor
{
    [SerializeField] private GameObject FirePref;       // префаб огня
    [SerializeField] private float fireZoneLifeTime;    // задержка перед исчезновением огненой зоны
    private bool created = false;                       // бул (сзодана ли огненая зона) для избежания многократного создания

    override protected void onDrop()
    {
            GameObject fireZone = Instantiate(FirePref, dropZone.transform.position, Quaternion.identity);
            Destroy(fireZone, fireZoneLifeTime);
    }
}