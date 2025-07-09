using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using System.Collections;





public class WaveManager : MonoBehaviour
{
    [SerializeField] List<UnitPath> paths = new List<UnitPath>();
    [SerializeField] WaveSegment wavePrefab;
    [SerializeField] Unit unitHeavyPrefab;
    [SerializeField] Unit unitDronePrefab;

    private void Awake()
    {
        // initialize wave data
        WaveSegment s1 = Instantiate<WaveSegment>(wavePrefab);
        s1.Init(paths[0], unitDronePrefab, 10, 5f);
        WaveSegment s1_p1 = Instantiate<WaveSegment>(wavePrefab);
        s1_p1.Init(paths[0], unitHeavyPrefab, 5, 15f);

        // add parallel wave
        s1.AddParallelWave(s1_p1, 20f);

        // play the waves
        s1.Play();
    }
}
