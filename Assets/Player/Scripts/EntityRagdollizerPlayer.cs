using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRagdollizerPlayer : EntityRagdollizer
{
    [SerializeField] GameObject weapons;
    void Start()
    {
        Deragdollize();
    }
    public override void Ragdollize()
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
        foreach(Rigidbody r in weapons.GetComponentsInChildren<Rigidbody>())
        {
            r.isKinematic = false;
        }
        foreach (Collider c in weapons.GetComponentsInChildren<Collider>())
        {
            c.enabled = true;
        }
        foreach(BarrelByInstantiation barrel in weapons.GetComponentsInChildren<BarrelByInstantiation>())
        {
            barrel.enabled = false;
        }
        foreach (BarrelByRaycast barrel in weapons.GetComponentsInChildren<BarrelByRaycast>())
        {
            barrel.enabled = false;
        }
    }
    public override void Deragdollize()
    {
        ragdollized = true;
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }
        foreach (Rigidbody r in rigidbodies)
        {
            r.isKinematic = true;
        }
        foreach (Rigidbody r in weapons.GetComponentsInChildren<Rigidbody>())
        {
            r.isKinematic = true;
        }
        foreach (Collider c in weapons.GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
        foreach (BarrelByInstantiation barrel in weapons.GetComponentsInChildren<BarrelByInstantiation>())
        {
            barrel.enabled = true;
        }
        foreach (BarrelByRaycast barrel in weapons.GetComponentsInChildren<BarrelByRaycast>())
        {
            barrel.enabled = true;
        }
    }
}
