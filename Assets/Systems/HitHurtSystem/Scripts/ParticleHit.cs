using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    [SerializeField] float damagePerParticle = 0.5f;
    private void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag != "Player")
        {
            other.GetComponent<Hurtbox>()?.NotifyHit(this, damagePerParticle);
        }
    }
}