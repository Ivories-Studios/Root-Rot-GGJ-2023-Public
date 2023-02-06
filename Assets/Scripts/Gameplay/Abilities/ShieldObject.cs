using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldObject : UnitObject
{
    private ShieldAbility _shield;

    protected override void Awake()
    {
        base.Awake();
        _shield = GetComponent<ShieldAbility>();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _shield.TakeDamage(damage);
    }
}
