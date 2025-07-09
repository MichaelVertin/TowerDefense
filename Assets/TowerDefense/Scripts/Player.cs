using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Player : MonoBehaviour
{
    private static TurretFactory __selectedTurretFactory = null;
    public static int __resources = 250;

    public static int Resources
    {
        get { return __resources; }
        set
        {
            __resources = value;
            if (__resources < 0) __resources = 0;
            Prefabs.instance.resourceTextField.text = __resources.ToString();
        }
    }

    public void Awake()
    {
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
                else
                {
                    Turret turret = turretPos.DetachTurret();
                    Destroy(turret.gameObject);
                }
            }
        }
    }

    public static void SelectTurret(TurretFactory factory)
    {
        __selectedTurretFactory = factory;
    }
}
