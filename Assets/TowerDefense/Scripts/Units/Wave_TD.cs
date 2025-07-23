using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_TD : MonoBehaviour
{
    private List<WaveSegment> waveSegments = new List<WaveSegment>();
    private List<WaveSegmentTiming> segmentTimings = new List<WaveSegmentTiming>();
    private bool isRunning = false;
    
    [System.Serializable]
    public class WaveSegmentTiming
    {
        public WaveSegment segment;
        public float startDelay;
        public bool delayFromWaveStart;
        public WaveSegment referencedSegment;
        
        public WaveSegmentTiming(WaveSegment segment, float startDelay, bool delayFromWaveStart = true, WaveSegment referencedSegment = null)
        {
            this.segment = segment;
            this.startDelay = startDelay;
            this.delayFromWaveStart = delayFromWaveStart;
            this.referencedSegment = referencedSegment;
        }
    }
    
    public void AddWaveSegment(WaveSegment segment, float startDelay, bool delayFromStart = true, WaveSegment referenceSegment = null)
    {
        waveSegments.Add(segment);
        segmentTimings.Add(new WaveSegmentTiming(segment, startDelay, delayFromStart, referenceSegment));
    }
    
    public void StartWave()
    {
        if (!isRunning)
        {
            StartCoroutine(RunWave());
        }
    }
    
    private IEnumerator RunWave()
    {
        isRunning = true;
        
        foreach (var timing in segmentTimings)
        {
            StartCoroutine(StartSegmentWithDelay(timing));
        }
        
        yield return null;
    }
    
    private IEnumerator StartSegmentWithDelay(WaveSegmentTiming timing)
    {
        if (timing.delayFromWaveStart)
        {
            // Delay From Wave Start -> Wait for wave to start spawning before delaying
            if(timing.referencedSegment != null)
            {
                yield return new WaitUntil(() => timing.referencedSegment.HasStartedSpawning);
            }
        }
        else if (timing.referencedSegment != null)
        {
            // Delay From Wave Start -> Wait for wave to finish spawning before delaying
            yield return new WaitUntil(() => timing.referencedSegment.HasFinishedSpawning);
        }
        // wait for startDelay after trigger condition has been met
        yield return new WaitForSeconds(timing.startDelay);
        timing.segment.StartWaveSegment();
    }
    
    public bool IsDone()
    {
        if (!isRunning) return false;
        
        foreach (var segment in waveSegments)
        {
            if (!segment.AreAllUnitsDestroyed())
            {
                return false;
            }
        }
        
        return true;
    }
}