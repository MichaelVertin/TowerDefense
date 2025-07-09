using Unity.VisualScripting;
using UnityEngine;

public abstract class Unit : PathFollower
{
    [SerializeField] private float StartingHealth;
    [SerializeField] protected float _health;
    [SerializeField] protected int _resourceValue;

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

    public void OnDeath()
    {
        Player.Resources += _resourceValue;
        Destroy(this.gameObject);
    }
}
