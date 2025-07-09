using UnityEngine;

public class TurretPosition : MonoBehaviour
{
    private Turret __turret = null;
    private TurretBasePlacementPoint __placementPoint;

    public void Awake()
    {
        __placementPoint = GetComponentInChildren<TurretBasePlacementPoint>();
    }

    public Turret Turret
    {
        get { return __turret; }
        set
        {
            SetTurret(value);
        }
    }

    // detaches and returns the turret on this
    // returns null if turret not attached
    public Turret DetachTurret()
    {
        // store current turret
        Turret turret = __turret;

        // detach the turret
        if(turret != null )
        {
            __turret = null;
        }

        // return stored turret
        return turret;
    }

    // fails if a turret is already set
    // places the turret on the placement point
    // sets the turret as the existing turret
    public bool SetTurret(Turret turret)
    {
        if (__turret)
        {
            return false;
        }

        // temporarily attach this as a parent to easily update the position
        Transform origParent = turret.transform.parent;
        turret.transform.parent = __placementPoint.transform;
        __turret = turret;
        __turret.transform.localPosition = Vector3.zero;
        __turret.transform.parent = origParent;
        return true;
    }
}
