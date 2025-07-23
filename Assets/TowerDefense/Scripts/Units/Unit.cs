using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Unit : PathFollower
{
    [SerializeField] private float StartingHealth;
    [SerializeField] protected int _resourceValue;
    [SerializeField] protected int _damage;
    protected float _health;

    private HealthBar __healthBar;

    private void Awake()
    {
        _health = StartingHealth;
        __healthBar = GetComponentInChildren<HealthBar>();
    }

    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            __healthBar.HealthRatio = _health / StartingHealth;
            if (_health < 0)
            {
                OnDeath();
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        foreach(Condition cond in new List<Condition>(_conditions))
        {
            cond.Update();
        }
    }

    // Called when unit dies due to health loss
    public void OnDeath()
    {
        Player_TD.Resources += _resourceValue;
        Destroy(this.gameObject);
    }

    // Called when unit reaches end of path
    public override void OnEndPath()
    {
        base.OnEndPath();
        Destroy(this.gameObject);
        Prefabs_TD.instance.UserBase.Health -= _damage;
    }

    #region Conditions
    protected List<Condition> _conditions = new List<Condition>();
    public void AddCondition(Condition condition)
    {
        Condition prevCond = _conditions.FirstOrDefault(c => c.ConditionType == condition.ConditionType);
        if (prevCond != null)
        {
            prevCond.SetDuration(condition.RemainingTime);
        }
        else
        {
            _conditions.Add(condition);
            condition.Apply();
        }
    }

    public void RemoveCondition(Condition condition)
    {
        Condition prevCond = _conditions.FirstOrDefault(c => c == condition);
        if( prevCond != null )
        {
            _conditions.Remove(prevCond);
        }
    }
    #endregion
}
