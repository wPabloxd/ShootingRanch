using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ExplodeOnEvent : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    Hitbox hitbox;
    private void Awake()
    {
        hitbox = GetComponent<Hitbox>();
    }
    private void OnEnable()
    {
        hitbox.onHit.AddListener(Explode);
        hitbox.OnCollisionWithoutHit.AddListener(Explode);
    }
    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnDisable()
    {
        hitbox.onHit.RemoveListener(Explode);
        hitbox.OnCollisionWithoutHit.RemoveListener(Explode);
    }
}
