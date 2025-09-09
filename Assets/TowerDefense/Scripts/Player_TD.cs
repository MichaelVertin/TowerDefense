using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Player_TD : MonoBehaviour
{
    private static TurretFactory __selectedTurretFactory = null;
    public static int __startingResources = 200;
    public static int __resources;

    public static int Resources
    {
        get { return __resources; }
        set
        {
            __resources = value;
            if (__resources < 0) __resources = 0;
            Prefabs_TD.instance.resourceTextField.text = __resources.ToString();
        }
    }

    public void Awake()
    {
        __resources = __startingResources;
        Resources = Resources;
    }

    public void Update()
    {

    }

    public static void PurchaseTurret(TurretFactory turretFactory, TurretPosition turretPos)
    {
        if (turretFactory.Purchase(ref __resources, out Turret turret))
        {
            Resources = Resources; // update UI
            turretPos.SetTurret(turret);
        }
    }
}
