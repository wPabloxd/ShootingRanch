using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EncounterChallenge : MonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] UnityEvent onWaveFinishedChallenge;
    WaveChallenge wave;
    protected int currentWave;
    private void Awake()
    {
        CustomAwake();
    }
    protected virtual void CustomAwake()
    {
        wave = GetComponentInChildren<WaveChallenge>();
        Enemy[] enemies = GetComponentsInChildren<Enemy>(true);
        foreach (Enemy e in enemies)
        {
            e.target = target;
        }
    }
    protected virtual void StartEncounter()
    {
        wave.StartWave();
    }
    private void Update()
    {
       UpdateWaves();
    }
    protected virtual void UpdateWaves()
    {
        if (wave.HasFinished())
        {
            wave.waveStarting = true;
            onWaveFinishedChallenge.Invoke();
            StartCoroutine(WavesDelay());
        }
    }
    protected virtual IEnumerator WavesDelay()
    {
        yield return new WaitForSeconds(3);
        StartEncounter();
    }

    public void NotifyTriggered()
    {
        StartEncounter();
    }
}

