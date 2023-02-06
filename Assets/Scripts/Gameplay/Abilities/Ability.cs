using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected UnitObject Caster;
    [HideInInspector] public Vector2 Target;

    public virtual void StartCasting(UnitObject unit)
    {
        Caster = unit;
    }

    public virtual void Cast()
    {
        gameObject.transform.position = Caster.transform.position;
        gameObject.transform.rotation = Caster.transform.rotation;
        gameObject.SetActive(true);
    }
}
