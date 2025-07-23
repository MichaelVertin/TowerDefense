using UnityEngine;
using System.Collections.Generic;

public class TurretAttackVisual_Bullet : TurretAttackVisual
{
    private List<WFX_LightFlicker> lightEffects;

    public override bool Attacking
    {
        set
        {
            // also set lightEffect
            base.Attacking = value;
            foreach(WFX_LightFlicker lightEffect in lightEffects)
            {
                lightEffect.Attacking = value;
            }
        }
    }

    public override void Awake()
    {
        lightEffects = new List<WFX_LightFlicker>(GetComponentsInChildren<WFX_LightFlicker>());
        base.Awake();
    }
}
