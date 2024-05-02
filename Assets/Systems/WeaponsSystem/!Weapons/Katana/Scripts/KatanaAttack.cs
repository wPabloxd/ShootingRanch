using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KatanaAttack : MonoBehaviour
{
    [SerializeField] EntityMeleeAttack entityMeleeAttack;
    [SerializeField] InputActionReference attackInput;
    [SerializeField] Animator animator;
    [SerializeField] Weapon weapon;
    [SerializeField] float attackCD;
    bool attackInCD;
    private void OnEnable()
    {
        attackInput.action.Enable();
    }
    void Update()
    {
        if(!attackInCD && attackInput.action.WasPerformedThisFrame())
        {
            weapon.currentAmmo--;
            animator.SetTrigger("MeleeAttack");
            StartCoroutine(AttackCD());
        }
    }
    public void Attack()
    {
        entityMeleeAttack.PerformAttack();
    }
    public void EndMelee()
    {
        animator.ResetTrigger("MeleeAttack");
    }
    IEnumerator AttackCD()
    {
        attackInCD = true;
        yield return new WaitForSeconds(attackCD);
        attackInCD = false;
    }
    private void OnDisable()
    {
        attackInput.action.Disable();
    }
}