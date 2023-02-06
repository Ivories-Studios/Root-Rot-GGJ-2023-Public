using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbility : Ability
{
    public int ShieldAmount;
    public float ShieldDuration;

    public override void StartCasting(UnitObject unit)
    {
        base.StartCasting(unit);
    }

    public override void Cast()
    {
        DestroyObject();
    }

    public void TakeDamage(float damage)
    {
        ShieldAmount -= (int)damage;
        if (ShieldAmount <= 0)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        ShieldDuration -= Time.deltaTime;
        if (ShieldDuration <= 0)
        {
            DestroyObject();
        }
    }
}
