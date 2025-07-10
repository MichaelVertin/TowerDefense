using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Turret_Flame : Turret
{
    [SerializeField] float burnDuration = 3.0f;
    [SerializeField] float burnDamage = 200f;
    [SerializeField] float burnFrequency = .5f;

    protected override void Attack(List<Unit> units)
    {
        base.Attack(units);
        if (units.Count > 0)
        {
            Unit unit = units[0];
            Condition condition = new BurnCondition(unit, burnDuration, burnDamage, burnFrequency);
            unit.AddCondition(condition);
        }
    }
}
