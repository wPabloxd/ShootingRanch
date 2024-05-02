using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRagdollizer : MonoBehaviour
{
    [SerializeField] bool startAsRagdoll;
    public Collider[] colliders;
    public Rigidbody[] rigidbodies;
    [Header("Debug")]
    [SerializeField] bool debugRagdollize;
    [SerializeField] bool debugDeragdollize;
    [SerializeField] Vector3 debugDirection;
    [SerializeField] bool debugPush;
    [SerializeField] float debugMinForce;
    [SerializeField] float debugMaxForce;
    public bool ragdollized;

    private void OnValidate()
    {
        if(debugRagdollize)
        {
            debugRagdollize = false;
            Ragdollize();
        }
        if (debugDeragdollize)
        {
            debugDeragdollize = false;
            Deragdollize();
        }
        if(debugPush)
        {
            debugPush = false;
            Push(debugDirection.normalized, debugMinForce, debugMaxForce);
        }
    }
    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }
    private void Start()
    {
        if (!startAsRagdoll)
        {
            Deragdollize();
        }
    }
    public virtual void Ragdollize()
    {
        ragdollized = true;
        foreach (Collider c in colliders)
        {
            c.enabled = true;
        }
        foreach (Rigidbody r in rigidbodies)
        {
            r.isKinematic = false;
        }
    }
    public virtual void Deragdollize()
    {
        ragdollized = false;
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }
        foreach (Rigidbody r in rigidbodies)
        {
            r.isKinematic = true;
        }
    }
    public void Push(Vector3 direction, float minForce, float maxForce)
    {
        int randomRB = UnityEngine.Random.Range(0, 12);
        int rbCounter = 0;
        foreach(Rigidbody r in rigidbodies)
        {
            if(randomRB == rbCounter)
            {
                r.AddForce(direction * UnityEngine.Random.Range(minForce, maxForce), ForceMode.Impulse);
                return;
            }
            rbCounter++;
        }
    }
}