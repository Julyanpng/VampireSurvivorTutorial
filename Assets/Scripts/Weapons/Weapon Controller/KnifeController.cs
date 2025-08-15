using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedKnife = Instantiate(weaponData.Prefab);
        spawnedKnife.transform.position = transform.position; //assign the position to be the same as this object which is parented to the player
        spawnedKnife.GetComponent<KnifeBehavior>().DirectionChecker(pm.lastMoveVector);//reference and set direction
    }
}
