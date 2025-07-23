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
        if(InputManager.WasLeftMouseButtonPressed)
        {
            if(InputManager.GetObjectUnderMouse<TurretPosition>(out TurretPosition turretPos))
            {
                if (turretPos.Turret == null)
                {
                    if(__selectedTurretFactory != null)
                    {
                        if(__selectedTurretFactory.Purchase(ref __resources, out Turret turret))
                        {
                            Resources = Resources; // update UI
                            turretPos.SetTurret(turret);
                        }
                    }
                }
            }
        }
    }

    public static void SelectTurret(TurretFactory factory)
    {
        if(__selectedTurretFactory)
        {
            __selectedTurretFactory.Selected = false;
        }
        __selectedTurretFactory = factory;
        __selectedTurretFactory.Selected = true;
    }
}
