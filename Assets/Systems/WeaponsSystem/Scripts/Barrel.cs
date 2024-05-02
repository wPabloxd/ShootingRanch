using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Barrel : MonoBehaviour
{
    public virtual void Shot() { }
    public virtual void StartShooting() { }
    public virtual void StopShooting() { }
    public virtual void BurstShooting() { }
}