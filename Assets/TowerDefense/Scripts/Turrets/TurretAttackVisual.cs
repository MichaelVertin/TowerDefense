using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TurretAttackVisual : MonoBehaviour
{
    private List<ParticleSystem> __particleSystems;

    public virtual bool Attacking
    {
        set
        {
            // enable/disable each particle system
            foreach (ParticleSystem ps in __particleSystems)
            {
                var emission = ps.emission;
                emission.enabled = value;
            }
        }
    }

    public virtual void Awake()
    {
        // initialize particle systems
        __particleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
        
        // disable at start
        Attacking = false;
    }
}
