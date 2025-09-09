using UnityEngine;

public class SelectionPanel : MonoBehaviour
{
    [SerializeField] private GameObject constructionPanel;

    public static SelectionPanel instance;
    private GameObject activePanel = null;
    private TurretPosition selectedPosition = null;

    private void Awake()
    {
        instance = this;
        constructionPanel.SetActive(false);
    }

    public void Update()
    {
        if (InputManager.WasLeftMouseButtonPressed)
        {
            if (!InputManager.GetObjectUnderMouse<SelectionPanel>(out SelectionPanel _))
            {
                Unselect();
            }

            if (InputManager.GetObjectUnderMouse<TurretPosition>(out TurretPosition turretPos))
            {
                SelectTurretPosition(turretPos);
            }
        }
    }
    private void SelectTurretPosition(TurretPosition turretPosition)
    {
        Turret turret = turretPosition.Turret;
        if (turret == null)
        {
            selectedPosition = turretPosition;
            selectedPosition.Select();
            activePanel = constructionPanel;
            activePanel.SetActive(true);
        }
    }

    public void Unselect()
    {
        if(activePanel != null)
        {
            if(selectedPosition) selectedPosition.Unselect();
            activePanel.SetActive(false);
            activePanel = null;
        }
    }

    public void ConstructTurret(TurretFactory factory)
    {
        if (selectedPosition == null) { return; }
        Player_TD.PurchaseTurret(factory, selectedPosition);
        Unselect();
    }
}
