using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

public class EntityLife : MonoBehaviour
{
    public UnityEvent <Vector3> died;
    [Header("Stats")]
    [SerializeField] float maxLife = 3f;
    [SerializeField] float minDeathPushForce;
    [SerializeField] float maxDeathPushForce;
    public float currentLife;

    [Header("Audio")]
    AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] bool isPlayer;

    [Header("Debug")]
    [SerializeField] bool debugDamage;

    HPBar hpBar;
    Animator anim;
    NavMeshAgent agent;
    Hurtbox hurtbox;
    Enemy enemy;
    EntityRagdollizer entityRagdollizer;
    PlayerController playerController;
    CharacterController characterController;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        hpBar = GetComponentInChildren<HPBar>();
        currentLife = maxLife;
        enemy = GetComponent<Enemy>();
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        hurtbox = GetComponent<Hurtbox>();
        anim = GetComponentInChildren<Animator>();
        entityRagdollizer = GetComponentInChildren<EntityRagdollizer>();
    }
    private void OnEnable()
    {
        hurtbox.onHitNotifiedWithOffender.AddListener(OnHitNotifiedWithOffender);
    }
    private void OnValidate()
    {
        if (debugDamage)
        {
            debugDamage = false;
            //OnHitNotifiedWithOffender(transform);
        }
    }
    public void OnHitNotifiedWithOffender(float damage, Transform offender)
    {
        if(currentLife > 0)
        {
            currentLife -= damage;
            if (isPlayer)
            {
                if (currentLife == 10)
                {
                    GameManager.Instance.spawnHealingProbability = 0;
                }
                else if (currentLife > 7.5f)
                {
                    GameManager.Instance.spawnHealingProbability = 5;
                }
                else if (currentLife > 5)
                {
                    GameManager.Instance.spawnHealingProbability = 10;
                }
                else if (currentLife > 2.5f)
                {
                    GameManager.Instance.spawnHealingProbability = 20;
                }
                else if (currentLife > 0f)
                {
                    GameManager.Instance.spawnHealingProbability = 40;
                }
            }
            int randomInt = UnityEngine.Random.Range(0, audioClips.Length);
            hpBar.SetNormalizedValue(Mathf.Clamp01(currentLife / maxLife), (currentLife / maxLife) * 100);
            if (isPlayer && damage > 0)
            {
                audioSource.clip = audioClips[randomInt];
                audioSource.Play();
            }
            if (currentLife <= 0)
            {
                if (characterController)
                {
                    characterController.enabled = false;
                }
                if (playerController)
                {
                    playerController.enabled = false;
                }
                if (agent)
                {
                    agent.speed = 0;
                    agent.velocity = Vector3.zero;
                    agent.enabled = false;
                }
                if (enemy)
                {
                    enemy.enabled = false;
                }
                anim.enabled = false;
            
                entityRagdollizer.Ragdollize();
                entityRagdollizer.Push(-(offender.position - transform.position).normalized, minDeathPushForce, maxDeathPushForce);
                died.Invoke(transform.position);
                if (!isPlayer)
                {
                    gameObject.GetComponent<CapsuleCollider>().enabled = false;
                    audioSource.clip = audioClips[randomInt];
                    audioSource.Play();
                    Destroy(gameObject, 3);
                }
            }
        }  
    }
    private void OnDisable()
    {
        hurtbox.onHitNotifiedWithOffender.RemoveListener(OnHitNotifiedWithOffender);
    }
}