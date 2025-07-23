using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSegment : MonoBehaviour
{
    [SerializeField] private Unit unitPrefab;
    [SerializeField] private int unitCount;
    [SerializeField] private float spawnInterval;
    [SerializeField] private UnitPath path;
    
    private List<Unit> spawnedUnits = new List<Unit>();
    private bool isSpawning = false;
    private bool hasFinishedSpawning = false;
    private bool hasStartedSpawning = false;

    public bool HasFinishedSpawning => hasFinishedSpawning;
    public bool HasStartedSpawning => hasStartedSpawning;
    public List<Unit> SpawnedUnits => spawnedUnits;
    
    public void Initialize(Unit unitPrefab, int unitCount, float spawnInterval, UnitPath path)
    {
        this.unitPrefab = unitPrefab;
        this.unitCount = unitCount;
        this.spawnInterval = spawnInterval;
        this.path = path;
    }
    
    public void StartWaveSegment()
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnUnits());
        }
    }
    
    private IEnumerator SpawnUnits()
    {
        isSpawning = true;
        hasStartedSpawning = true;
        
        for (int i = 0; i < unitCount; i++)
        {
            SpawnUnit();
            
            if (i < unitCount - 1) // Don't wait after the last unit
            {
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        
        hasFinishedSpawning = true;
        isSpawning = false;
    }
    
    private void SpawnUnit()
    {
        Unit newUnit = Instantiate(unitPrefab);
        newUnit.Init(path);
        spawnedUnits.Add(newUnit);
    }
    
    public bool AreAllUnitsDestroyed()
    {
        spawnedUnits.RemoveAll(unit => unit == null);
        return spawnedUnits.Count == 0 && hasFinishedSpawning;
    }
}