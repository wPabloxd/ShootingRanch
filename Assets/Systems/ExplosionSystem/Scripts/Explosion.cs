using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float range = 5;
    [SerializeField] float force = 1000;
    [SerializeField] float maxDamage = 15;
    [SerializeField] float upwardsModifier = 1000;
    [SerializeField] LayerMask layerMask = Physics.DefaultRaycastLayers;
    [SerializeField] GameObject visualExplosionPrefab;

    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, layerMask);
        foreach(Collider c in colliders)
        {
            if(c.gameObject.tag != "Player")
            {
                if (c.TryGetComponent<Hurtbox>(out Hurtbox hb))
                {
                    hb.NotifyHit(this, CalculateDamage(c.gameObject));
                }
                if (c.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.AddExplosionForce(force, transform.position, range, upwardsModifier);
                }
            }     
        }
        Instantiate(visualExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    float CalculateDamage(GameObject damagedObject)
    {
        float distance = Vector3.Distance(damagedObject.transform.position, transform.position);
        float damageMultiplier = Mathf.Clamp01(1f - Mathf.Log10(distance) / Mathf.Log10(range));
        float actualDamage = damageMultiplier * maxDamage;
        return actualDamage;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}