using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Encounter : EncounterChallenge
{
    [SerializeField] Transform encounterDoorParent;
    [SerializeField] bool lastEncounter;
    [SerializeField] UnityEvent onEncounterFinished;
    [SerializeField] UnityEvent onWaveFinished;
    Wave[] waves;
    [Header("Debug")]
    [SerializeField] bool debugStartEncounter;
    [SerializeField] bool debugHasFinished;
    private void OnValidate()
    {
        if (debugStartEncounter)
        {
            debugStartEncounter = false;
            StartEncounter();
        }
    }
    protected override void CustomAwake()
    {
        waves = GetComponentsInChildren<Wave>();
        Enemy[] enemies = GetComponentsInChildren<Enemy>(true);
        foreach (Enemy e in enemies)
        {
            e.target = target;
        }
    }
    protected override void StartEncounter()
    {
        SetDoorsActivation(true);
        waves[currentWave].StartWave();
    }
    protected override void UpdateWaves()
    {
        if (currentWave < waves.Length)
        {
            if (waves[currentWave].HasFinished())
            {
                currentWave++;
                if (currentWave < waves.Length)
                {
                    onWaveFinished.Invoke();
                    StartCoroutine(WavesDelay());
                }
                else
                {
                    onEncounterFinished.Invoke();
                    if (lastEncounter)
                    {
                        GameManager.Instance.songPhase = 4;
                    }
                    else
                    {
                        GameManager.Instance.songPhase = 3;
                    }
                    SetDoorsActivation(false);
                }
            }
        }
        debugHasFinished = HasFinished();
    }
    protected override IEnumerator WavesDelay()
    {
        yield return new WaitForSeconds(3);
        waves[currentWave].StartWave();
    }
    protected bool HasFinished()
    {    
        return currentWave >= waves.Length;
    }
    void SetDoorsActivation(bool activation)
    {
        foreach (Transform door in encounterDoorParent)
        {
            door.gameObject.SetActive(activation);
        }
    }
}
