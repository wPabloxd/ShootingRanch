using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEntityAnimable
{
    public Transform target;
    [SerializeField] float attackDistance = 1;
    [SerializeField] float acceleration = 60f;
    EntityAnimation entityAnimation;
    NavMeshAgent agent;
    Animator animator;
    float initialSpeed;
    public float rotationSpeed = 720f;
    AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    public bool emitingSound;
    bool noiseCD = true;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        entityAnimation = GetComponent<EntityAnimation>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        StartCoroutine(NoiseCD());
        initialSpeed = agent.speed;
    }
    void Update()
    {
        if(!emitingSound && !noiseCD) 
        {
            int randomInt = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomInt];
            audioSource.Play();
            StartCoroutine(NoiseCD());
        }
        Vector3 destination = target ? target.position : transform.position;
        agent.SetDestination(destination);
        if (target)
        {
            animator.SetBool("IsAttacking", target ? (Vector3.Distance(transform.position, target.position) < attackDistance) : false);
            Quaternion targetRotation;
            if (agent.velocity != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(Vector3.forward);
            }
            if (Vector3.Distance(transform.position, target.position) < attackDistance)
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                targetRotation = Quaternion.LookRotation(directionToTarget);
                entityAnimation.closeToPlayer = true;
                agent.speed -= acceleration * Time.deltaTime;
            }
            else if(agent.speed < initialSpeed)
            {
                agent.speed += acceleration * Time.deltaTime;
            }
            else
            {
                agent.speed = initialSpeed;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    IEnumerator NoiseCD()
    {
        noiseCD = true;
        float randomCD = Random.Range(0f, 6f);
        yield return new WaitForSeconds(randomCD);
        noiseCD = false;
    }
    public Vector3 GetLastVelocity()
    {
        return agent.velocity;
    }
    public float GetVerticalVelocity()
    {
        return 0;
    }
    public float GetJumpSpeed()
    {
        return 0;
    }
    public bool IsGrounded()
    {
        return true;
    }
}
