using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMeteor : Meteor
{
    [SerializeField] private GameObject FirePref; // префаб огня
    private bool created = false;                 // бул (сзодана ли огненая зона) для избежания многократного создания


    // костыль (выполняется Update() метеора и FixedUpdate() текущего класса
    // переписать под обсервер
    private void FixedUpdate()
    {
        if (!created && Vector2.Distance(rb.position, dropPoint) < 1f)
        {
            created = true;
            Instantiate(FirePref, dropZone.transform.position, Quaternion.identity);
        }
    }
}