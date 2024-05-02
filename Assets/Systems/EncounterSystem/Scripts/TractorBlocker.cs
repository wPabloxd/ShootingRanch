using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBlocker : MonoBehaviour
{
    [SerializeField] bool enter;
    TractorTrigger tractorTrigger;
    private void Awake()
    {
        tractorTrigger = GetComponentInParent<TractorTrigger>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (enter)
            {
                tractorTrigger.Activate();
            }
            else
            {
                tractorTrigger.Deactivate();
            }
        }        
    }
}
