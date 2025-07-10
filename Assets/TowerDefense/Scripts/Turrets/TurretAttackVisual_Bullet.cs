using UnityEngine;

public class TurretAttackVisual_Bullet : TurretAttackVisual
{
    private WFX_LightFlicker lightEffect;

    public override bool Attacking
    {
        set
        {
            // also set lightEffect
            base.Attacking = value;
            lightEffect.Attacking = value;
        }
    }

    public override void Awake()
    {
        lightEffect = GetComponentInChildren<WFX_LightFlicker>();
        base.Awake();
    }
}
