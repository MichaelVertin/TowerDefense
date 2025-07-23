using TMPro;
using UnityEditor;
using UnityEngine;

public class Base_TD : MonoBehaviour
{
    [SerializeField] private TMP_Text __baseHealthField;
    [SerializeField] private int __initialHealth;
    private int __health;

    public int Health
    {
        get { return __health; }
        set
        {
            __health = value;
            __baseHealthField.text = "Health: " + __health.ToString();
            if(__health <= 0)
            {
                Prefabs_TD.instance.endGameManager.OnGameEnd(won: false);
            }
        }
    }

    public void Awake()
    {
        Health = __initialHealth;
    }
}
