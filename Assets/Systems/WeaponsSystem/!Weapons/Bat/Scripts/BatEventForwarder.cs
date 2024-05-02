using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEventForwarder : MonoBehaviour
{
    Animator animator;
    PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();
    }
    public void EndMelee()
    {
        playerController.ResetWeapon();
        animator.ResetTrigger("MeleeAttack");
    }
}