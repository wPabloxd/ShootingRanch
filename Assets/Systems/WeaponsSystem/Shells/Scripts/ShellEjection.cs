using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEjection : MonoBehaviour
{
    Rigidbody rigidBody;
    CapsuleCollider capsuleCollider;
    private void Awake()
    {
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        capsuleCollider.enabled = false;
        rigidBody.AddRelativeForce(Vector3.forward * Random.Range(2.5f, 3.5f), ForceMode.Impulse);
        Destroy(gameObject, 2);
        StartCoroutine(ColliderPreparation());
    }
    IEnumerator ColliderPreparation()
    {
        yield return new WaitForSeconds(0.5f);
        capsuleCollider.enabled = true;
    }
}