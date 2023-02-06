using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAbilityWrapper : MonoBehaviour
{
    [SerializeField] private UnitObject UnitObject;

    public void CastAbility(int index)
    {
        UnitObject.CastAbility(index);
    }
}
