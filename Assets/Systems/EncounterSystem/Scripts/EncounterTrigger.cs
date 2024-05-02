using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterTrigger : MonoBehaviour
{
    [SerializeField] bool challenge;
    Encounter encounter;
    EncounterChallenge encounterChallenge;
    private void Awake()
    {
        if(challenge)
        {
            encounterChallenge = GetComponentInParent<EncounterChallenge>();
        }
        else
        {
            encounter = GetComponentInParent<Encounter>();
        }
    }
    public void StartEncounter()
    {
        if (challenge)
        {
            encounterChallenge.NotifyTriggered();
        }
        else
        {
            encounter.NotifyTriggered();
        }
        Destroy(gameObject.GetComponent<EncounterTrigger>());
    }
}