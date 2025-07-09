using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class WaveSegment : MonoBehaviour
{
    private int unitCount;
    private float timeBetweenUnits;
    private Unit unitToSpawn;
    private UnitPath unitPath;
    private bool __done = false;

    private List<Tuple<WaveSegment, float>> parallelWaves = new List<Tuple<WaveSegment, float>>();
    private Tuple<WaveSegment, float> sequentialWave = null;

    public void Init(UnitPath path, Unit unitToSpawn, int count, float timeBetweenSpawns)
    {
        this.unitToSpawn = unitToSpawn;
        this.unitPath = path;
        this.unitCount = count;
        this.timeBetweenUnits = timeBetweenSpawns;
    }

    public void Play(float delay = 0f)
    {
        StartCoroutine(PlayMainWave(delay));
        foreach (Tuple<WaveSegment, float> wave in parallelWaves)
        {
            wave.Item1.Play(delay + wave.Item2);
        }
    }

    public void AddSequentialWave(WaveSegment waveSegment, float delay = 0f)
    {
        if (sequentialWave != null)
        {
            sequentialWave.Item1.AddSequentialWave(waveSegment, delay);
            return;
        }

        sequentialWave = new Tuple<WaveSegment, float>(waveSegment, delay);
    }

    public void AddParallelWave(WaveSegment waveSegment, float delay = 0f)
    {
        parallelWaves.Add(new Tuple<WaveSegment, float>(waveSegment, delay));
    }

    public bool Done()
    {
        // not done if main loop not done
        if (!__done) return false;
        // not done if the next wave is not done
        if (sequentialWave != null)
        {
            return sequentialWave.Item1.Done();
        }

        // done if no sequential wave exists
        return true;
    }

    private IEnumerator PlayMainWave(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int spawnCount = 0; spawnCount < unitCount; spawnCount++)
        {
            Unit unit = Instantiate<Unit>(unitToSpawn);
            unit.Init(unitPath);

            yield return new WaitForSeconds(timeBetweenUnits);
        }

        if (sequentialWave != null)
        {
            yield return new WaitForSeconds(sequentialWave.Item2);
            sequentialWave.Item1.Play();
        }
        __done = true;
        yield return null;
    }
}
