using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Turret_Freeze : Turret
{
    [SerializeField] float slowDuration = 3.0f;
    [SerializeField] float slowStrength = .5f;
    protected override void Attack(List<Unit> units)
    {
        base.Attack(units);
        if(units.Count > 0)
        {
            Unit unit = units[0];
            Condition condition = new SlowCondition(unit, slowDuration, slowStrength);
            unit.AddCondition(condition);
        }
    }
}
