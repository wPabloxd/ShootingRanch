using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityMeleeAttack : MonoBehaviour
{
    [Header("Attack Stats")]
    [SerializeField] Vector3 offset = Vector3.forward + Vector3.up;
    [SerializeField] float radius = 0.35f;
    [SerializeField] LayerMask layerMask = Physics.DefaultRaycastLayers;
    [SerializeField] string[] affectedTags = { "Untagged", "Player" };
    [SerializeField] float damage = 1;

    [Header("Melee Kind")]
    [SerializeField] bool isEnemy;

    [Header("Sound")]
    AudioSource audioSource;
    [SerializeField] AudioClip[] audioClipsHit;
    [SerializeField] AudioClip[] audioClipMiss;
    Enemy enemy;
    bool soundCD;
    [Header("Debug")]
    [SerializeField] bool debugAttack;

    private void Awake()
    {
        if(isEnemy)
        {
            enemy = GetComponent<Enemy>();
        }
        audioSource = GetComponent<AudioSource>(); 
    }
    private void OnValidate()
    {
        if (debugAttack)
        {
            debugAttack = false;
            PerformAttack();
        }
    }
    public void PerformAttack()
    {
        if (!isEnemy)
        {
            int randomInt = Random.Range(0, audioClipMiss.Length);
            audioSource.clip = audioClipMiss[randomInt];
            audioSource.Play();
        }
 
        Collider[] potentialHurtboxes = Physics.OverlapSphere(CalculateAttackPosition(), radius, layerMask);
        foreach(Collider c in potentialHurtboxes)
        {
            if(affectedTags.Contains(c.tag))
            {
                Hurtbox hurtbox = c.GetComponent<Hurtbox>();
                hurtbox?.NotifyHit(this, damage);
                if (!soundCD)
                {
                    int randomInt = Random.Range(0, audioClipsHit.Length);
                    audioSource.clip = audioClipsHit[randomInt];
                    audioSource.Play();
                    if (isEnemy)
                    {
                        StartCoroutine(SoundCD());
                        enemy.emitingSound = true;
                    }
                }
            }
        }
    }
    Vector3 CalculateAttackPosition()
    {
        return transform.position + transform.TransformDirection(offset);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 gizmoPos = transform.position+ transform.TransformDirection(offset);
        Gizmos.DrawWireSphere(gizmoPos, radius);
    }
    IEnumerator SoundCD()
    {
        soundCD = true;
        float randomCD = Random.Range(1f, 3f);
        yield return new WaitForSeconds(randomCD);
        soundCD = false;
        if (isEnemy)
        {
            enemy.emitingSound = false;
        }
    }
}