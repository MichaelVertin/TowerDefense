using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class Turret : MonoBehaviour
{
    [SerializeField] float fireRate;
    [SerializeField] float damage;
    private TurretRange __range;
    private TurretAttackVisual __attackVisual;

    public void Awake()
    {
        __range = GetComponentInChildren<TurretRange>();
        __attackVisual = GetComponentInChildren<TurretAttackVisual>();

        StartCoroutine(Attack_Coroutine());
    }

    // Rotates the rotatable part of the turret to look at the transform naturally
    public void LookAt(Transform targetTransform)
    {
        // get the rorational part of the turret
        TurretRotationGO_Tag rotatableGO = GetComponentInChildren<TurretRotationGO_Tag>();

        // identify the target position to look at by rotating around the xz plane
        //  - set the target to the transform's x and z
        //  - keep y
        var targetPosition = targetTransform.position;
        targetPosition.y = rotatableGO.transform.position.y;

        // set the rotational part of this turret to look at the target position
        rotatableGO.transform.LookAt(targetPosition);
    }

    public void FixedUpdate()
    {
        List<Unit> units = __range.Units;
        if (units.Count > 0)
        {
            Unit unit = units[0];

            // face the first unit
            LookAt(unit.transform);

            __attackVisual.Attacking = true;
        }
        else
        {
            __attackVisual.Attacking = false;
        }
    }

    private IEnumerator Attack_Coroutine()
    {
        while (true)
        {
            this.Attack(__range.Units);
            yield return new WaitForSeconds(fireRate);
        }
    }

    protected virtual void Attack(List<Unit> units)
    {
        if (units.Count > 0)
        {
            units[0].Health -= damage;
        }
    }
}
