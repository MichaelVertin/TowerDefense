// Assets/TowerDefense/Scripts/Conditions/BurnCondition.cs
using UnityEngine;
using System.Collections;

public abstract class Condition
{
    public abstract string ConditionType { get; }

    private float __endTime;
    protected Unit unit;

    public float RemainingTime
    {
        get { return __endTime - Time.time; }
    }
    public bool Done
    {
        get { return RemainingTime <= 0f; }
    }



    public Condition(Unit affectedUnit, float duration)
    {
        unit = affectedUnit;
        SetDuration(duration);
    }

    public virtual void Update()
    {
        if(Done)
        {
            Unapply();
        }
    }

    public void SetDuration(float duration)
    {
        __endTime = Time.time + duration;
    }

    public virtual void Apply()
    {

    }

    public virtual void Unapply()
    {
        if (unit != null)
        {
            unit.RemoveCondition(this);
        }
    }
}


public class BurnCondition : Condition
{
    public override string ConditionType => "Burn";

    private float burnDamage;
    private float burnFrequency;
    private float nextBurnTime;

    public BurnCondition(Unit unit, float duration, float burnDamage, float burnFrequency)
        : base(unit, duration)
    {
        this.burnDamage = burnDamage;
        this.burnFrequency = burnFrequency;
        this.nextBurnTime = Time.time + burnFrequency;
    }

    public override void Apply()
    {
        base.Apply();
    }

    public override void Unapply()
    {
        base.Unapply();
    }

    public override void Update()
    {
        base.Update();
        if(Time.time >= nextBurnTime)
        {
            unit.Health -= burnDamage;
            nextBurnTime += burnFrequency;
        }
    }
    /*
    protected override IEnumerator ConditionCoroutine()
    {
        float elapsed = 0f;
        
        while (elapsed < Duration && targetUnit != null)
        {
            targetUnit.Health -= burnDamage;
            yield return new WaitForSeconds(burnFrequency);
            elapsed += burnFrequency;
        }
        
        targetUnit.RemoveCondition(this);
    }
    */
}


public class SlowCondition : Condition
{
    public override string ConditionType => "Slow";

    private float __strength;
    private float __originalSpeed;

    public SlowCondition(Unit unit, float duration, float strength)
            : base(unit, duration)
    {
        __strength = strength;
    }


    public override void Apply()
    {
        base.Apply();
        __originalSpeed = unit.Speed;
        float slowMultiplier = 1f - __strength;
        unit.Speed = __originalSpeed * slowMultiplier;
    }

    public override void Unapply()
    {
        base.Unapply();
        if (unit != null)
        {
            unit.Speed = __originalSpeed;
        }
    }
}