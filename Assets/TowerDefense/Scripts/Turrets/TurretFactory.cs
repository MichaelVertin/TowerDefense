using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TurretFactory : MonoBehaviour
{
    [SerializeField] private TMP_Text __turretCostField;
    [SerializeField] private Turret __turretPrefab;
    [SerializeField] private int __initialCost = 100;
    private int __currentCost;
    private Outline __outline;

    public bool Selected
    {
        set { __outline.enabled = value; }
    }

    private void Awake()
    {
        // set initial cost
        Cost = __initialCost;
        __outline = GetComponent<Outline>();
        Selected = false;
    }


    private int Cost
    {
        get { return __currentCost; }
        set 
        {
            // when changing cost, update UI text
            __currentCost = value;
            __turretCostField.text = Cost.ToString();
        }
    }

    // Purchase the turret if enough resources are provided
    // turret is set to a new instance of turret if there are enough resources
    // otherwise, turret is set to null
    public bool Purchase(ref int resources, out Turret turret)
    {
        if(resources >= Cost)
        {
            resources -= Cost;
            Cost = (int)(Cost * 2.0f);
            turret = Instantiate<Turret>(__turretPrefab);
            return true;
        }
        turret = null;
        return false;
    }
}
