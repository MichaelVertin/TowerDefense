using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Prefabs : MonoBehaviour
{
    public static Prefabs instance;

    [SerializeField] public TMP_Text resourceTextField;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}