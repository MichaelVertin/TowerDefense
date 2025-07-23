using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Prefabs_TD : MonoBehaviour
{
    public static Prefabs_TD instance;

    [SerializeField] public TMP_Text resourceTextField;
    [SerializeField] public Base_TD UserBase;
    [SerializeField] public EndGameManager endGameManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}