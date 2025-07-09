using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TurretRange : MonoBehaviour
{
    private List<Unit> __units = new List<Unit>();

    public List<Unit> Units
    {
        get 
        {
            // remove all destroyed units
            __units.RemoveAll(x => x == null); 
            return __units;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        UnitBody body = other.gameObject.GetComponent<UnitBody>();
        if (body != null)
        {
            Unit unit = body.GetComponentInParent<Unit>();
            __units.Add(unit);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        UnitBody body = other.gameObject.GetComponent<UnitBody>();
        if (body != null)
        {
            Unit unit = body.GetComponentInParent<Unit>();
            __units.Remove(unit);
        }
    }
}
