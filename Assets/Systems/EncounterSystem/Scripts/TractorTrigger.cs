using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class TractorTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] lights;
    AudioSource audioSource;
    EncounterTrigger encounterTrigger;
    public bool alreadyTriggered;
    [SerializeField] bool ready;
    private void Awake()
    {
        encounterTrigger = GetComponent<EncounterTrigger>();
        audioSource = GetComponent<AudioSource>();
    }
    public void Activate()
    {
        ready = true;
    }
    public void Deactivate()
    {
        ready = false;
    }
    public void TractorStartUp()
    {
        if(ready)
        {
            if (alreadyTriggered) return;
            GameManager.Instance.songPhase = 1;
            alreadyTriggered = true;
            audioSource.Play();
            foreach (GameObject light in lights)
            {
                light.SetActive(true);
            }
            StartCoroutine(StartDelay());
        }
    }
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(2);
        encounterTrigger.StartEncounter();
    }
}