using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using System.Collections;
using TMPro;
using static UnityEngine.Rendering.HableCurve;

public class WaveManager_TD : MonoBehaviour
{
    [SerializeField] List<UnitPath> paths = new List<UnitPath>();
    [SerializeField] Unit unitHeavyPrefab;
    [SerializeField] Unit unitDronePrefab;
    [SerializeField] Unit unitMiniPrefab;
    [SerializeField] private TMP_Text __waveProgressField;

    private List<Wave_TD> waves = new List<Wave_TD>();
    private int currentWaveIndex;
    private bool isRunning = false;
    private int CurrentWaveIndex
    {
        get { return currentWaveIndex; }
        set
        {
            currentWaveIndex = value;
            __waveProgressField.text = "Waves: " + (currentWaveIndex + 1).ToString() + " / " + waves.Count.ToString();
        }
    }

    private void Awake()
    {
        CurrentWaveIndex = 0;
        CreateWaves();
    }
    
    private void Start()
    {
        StartWaves();
    }
    
    private void CreateWaves()
    {
        ConstructNewWave();
        WaveSegment w1_s1 = AddMiniWS(unitCount: 10, spawnInterval: 5f, delay: 5f);
        WaveSegment w1_s2 = AddDroneWS(unitCount: 5, spawnInterval: 10f, delay: 20f);

        ConstructNewWave();
        WaveSegment w2_s1 = AddDroneWS(unitCount: 5, spawnInterval: 5f, delay: 5f);
        WaveSegment w2_s2 = AddDroneWS(unitCount: 10, spawnInterval: 2.5f, delayType: DELAY_TYPE.FROM_END, delay: 5f, relativeTo: w2_s1);

        ConstructNewWave();
        WaveSegment w3_s1 = AddHeavyWS();

        ConstructNewWave();
        WaveSegment w4_s1 = AddDroneWS(unitCount: 30, spawnInterval: 5f);
        WaveSegment w4_s2 = AddHeavyWS(unitCount: 5, spawnInterval: 25f, delay: 10f, relativeTo: w4_s1);

        ConstructNewWave();
        WaveSegment w5_s1 = AddMiniWS(unitCount: 30, spawnInterval: 1f);
        WaveSegment w5_s2 = AddDroneWS(unitCount: 10, spawnInterval: 3f);

        ConstructNewWave();
        WaveSegment w6_s1 = AddHeavyWS(unitCount: 5, spawnInterval: 10f);

        ConstructNewWave();
        WaveSegment w7_s1 = AddHeavyWS(unitCount: 5, spawnInterval: 10f);
        WaveSegment w7_s2 = AddDroneWS(unitCount: 10, spawnInterval: 2f);
        WaveSegment w7_s3 = AddDroneWS(unitCount: 20, spawnInterval: 1f, relativeTo: w7_s2, delayType: DELAY_TYPE.FROM_END);
        WaveSegment w7_s4 = AddDroneWS(unitCount: 40, spawnInterval: .5f, relativeTo: w7_s3, delayType: DELAY_TYPE.FROM_END);
        WaveSegment w7_s5 = AddHeavyWS(unitCount: 20, spawnInterval: 5f, relativeTo: w7_s1, delayType: DELAY_TYPE.FROM_END, delay: 10f);

        ConstructNewWave();
        WaveSegment w8_s1 = AddHeavyWS(unitCount: 50, spawnInterval: 3f);
        WaveSegment w8_s2 = AddDroneWS(unitCount: 75, spawnInterval: 2f);
        WaveSegment w8_s3 = AddMiniWS(unitCount: 150, spawnInterval: 1f);

        CurrentWaveIndex = 0;
    }
    
    private WaveSegment CreateWaveSegment(Unit unitPrefab, int unitCount, float spawnInterval, UnitPath path, string name = "Wave Segment")
    {
        GameObject segmentObj = new GameObject(name);
        segmentObj.transform.SetParent(this.transform);
        WaveSegment segment = segmentObj.AddComponent<WaveSegment>();
        segment.Initialize(unitPrefab, unitCount, spawnInterval, path);
        return segment;
    }
    
    private void StartWaves()
    {
        if (!isRunning && waves.Count > 0)
        {
            StartCoroutine(RunWaves());
        }
    }
    
    private IEnumerator RunWaves()
    {
        isRunning = true;
        CurrentWaveIndex = 0;
        while (CurrentWaveIndex < waves.Count)
        {
            Debug.Log(CurrentWaveIndex);
            Wave_TD currentWave = waves[CurrentWaveIndex];
            currentWave.StartWave();
            
            yield return new WaitUntil(() => currentWave.IsDone());
            
            CurrentWaveIndex++;
            Debug.Log(CurrentWaveIndex);
        }
        
        isRunning = false;
        Prefabs_TD.instance.endGameManager.OnGameEnd(won: true);
        Debug.Log("All waves completed!");
    }

    #region WaveConstructor
    // TODO: move this to a separate class
    enum DELAY_TYPE { FROM_START, FROM_END };
    private Wave_TD waveInConstruction = null;
    private WaveSegment AddMiniWS(int unitCount = 1, float spawnInterval = 5f, DELAY_TYPE delayType = DELAY_TYPE.FROM_START, float delay = 0.0f, WaveSegment relativeTo = null)
    {
        return AddNewWS(unitMiniPrefab, unitCount, spawnInterval, delayType, delay, relativeTo);
    }
    
    private WaveSegment AddDroneWS(int unitCount = 1, float spawnInterval = 5f, DELAY_TYPE delayType = DELAY_TYPE.FROM_START, float delay = 0.0f, WaveSegment relativeTo = null)
    {
        return AddNewWS(unitDronePrefab, unitCount, spawnInterval, delayType, delay, relativeTo);
    }

    private WaveSegment AddHeavyWS(int unitCount = 1, float spawnInterval = 5f, DELAY_TYPE delayType = DELAY_TYPE.FROM_START, float delay = 0.0f, WaveSegment relativeTo = null)
    {
        return AddNewWS(unitHeavyPrefab, unitCount, spawnInterval, delayType, delay, relativeTo);
    }

    private WaveSegment AddNewWS(Unit unitPrefab, int unitCount = 1, float spawnInterval = 5f, DELAY_TYPE delayType = DELAY_TYPE.FROM_START, float delay = 0.0f, WaveSegment relativeTo = null)
    {
        bool delayFromStart = (delayType == DELAY_TYPE.FROM_START);
        WaveSegment ws = CreateWaveSegment(unitPrefab, unitCount: unitCount, spawnInterval: spawnInterval, paths[0]);
        waveInConstruction.AddWaveSegment(ws, delay, delayFromStart, relativeTo);
        return ws;
    }

    private void ConstructNewWave()
    {
        GameObject waveObj = new GameObject("Wave3");
        Wave_TD wave = waveObj.AddComponent<Wave_TD>();
        waves.Add(wave);
        waveInConstruction = wave;
    }
    #endregion
}