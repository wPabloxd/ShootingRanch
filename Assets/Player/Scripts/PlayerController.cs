using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour, IEntityAnimable
{
    public enum MovementMode
    {
        RelativeToCharacter,
        RelativeToCamera,
    };

    public enum OrientationMode
    {
        OrientateToCameraForward,
        OrientateToMovementForward,
        OrientateToTarget,
    }

    [Header("Movement Settings")]
    [SerializeField] float planeSpeed = 3f;
    [SerializeField] MovementMode movementMode = MovementMode.RelativeToCamera;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] bool paused;

    [Header("Orientation Settings")]
    [SerializeField] float angularSpeed = 360f;
    [SerializeField] Transform orientationTarget;
    [SerializeField] OrientationMode orientationMode = OrientationMode.OrientateToMovementForward;

    [Header("Movement Inputs")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;

    [Header("Weapon Inputs")]
    [SerializeField] InputActionReference swapWeapon;
    [SerializeField] InputActionReference fire;
    [SerializeField] InputActionReference meleeAttack;
    [SerializeField] InputActionReference[] selectWeaponInputs;
    [SerializeField] int batCharges = 3;
    Animator animator;
    CharacterController characterController;
    EntityWeapons entityWeapons;

    [Header("Bat")]
    [SerializeField] bool challenge;
    [SerializeField] Image chargeUI;
    [SerializeField] GameObject[] chargesReadyUI;
    bool alreadyRecharging;
    bool alreadyAttacking;
    int weaponBeforeBat;
    int batReloadQueue;
    bool isMelee;
    float fillAmountBat = 1;

    Vector3 velocityToApply = Vector3.zero;
    float verticalVelocity = 0f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        entityWeapons = GetComponent<EntityWeapons>();
    }
    private void OnEnable()
    {
        move.action.Enable();
        jump.action.Enable();
        meleeAttack.action.Enable();
        fire.action.Enable();
        swapWeapon.action.Enable();
        foreach (InputActionReference iar in selectWeaponInputs)
        {
            iar.action.Enable();
        }
    }
    void Update()
    {
        paused = GameManager.Instance.paused;
        if(!paused)
        {
            velocityToApply = Vector3.zero;
            UpdateMovementOnPlane();
            UpdateVerticalMovement();

            characterController.Move(velocityToApply * Time.deltaTime);

            UpdateOrientation();
            UpdateWeapons();
            if (!challenge)
            {
                BatWeapon();
                SwapWeapons();
                CurrentCDAmount();
            }
            else
            {
                PickWeapon();
            }
        }
    }
    public void ResetWeapon()
    {
        alreadyAttacking = false;
        entityWeapons.SetCurrentWeapon(weaponBeforeBat);
    }
    private void UpdateWeapons()
    {
        if (entityWeapons.HasCurrentWeapon())
        {
            switch (entityWeapons.GetCurrentWeapon().weaponKind)
            {
                case Weapon.WeaponKind.pistol:
                    animator.SetInteger("WeaponKind", 0);
                    isMelee = false;
                    break;
                case Weapon.WeaponKind.rifle:
                    animator.SetInteger("WeaponKind", 1);
                    isMelee = false;
                    break;
                case Weapon.WeaponKind.melee:
                    animator.SetInteger("WeaponKind", 2);
                    isMelee = true;
                    break;
            }
            if(!isMelee)
            {
                switch (entityWeapons.GetCurrentWeapon().shotMode)
                {
                    case Weapon.ShotMode.semiauto:
                        if (fire.action.WasPerformedThisFrame())
                        {
                            entityWeapons.Shot();
                        }
                        break;
                    case Weapon.ShotMode.fullauto:
                        if (fire.action.WasPerformedThisFrame())
                        {
                            entityWeapons.StartShooting();
                        }
                        if (fire.action.WasReleasedThisFrame())
                        {
                            entityWeapons.StopShooting();
                        }
                        break;
                    case Weapon.ShotMode.burstmode:
                        if (fire.action.WasPerformedThisFrame())
                        {
                            entityWeapons.BurstShooting();
                        }
                        break;
                }
            }   
        }
    }
    void SwapWeapons()
    {
        Vector2 swapWeaponValue = swapWeapon.action.ReadValue<Vector2>();
        if (!alreadyAttacking)
        {
            if (swapWeaponValue.y < 0)
            {
                entityWeapons.SelectNextWeapon();
            }
            else if (swapWeaponValue.y > 0)
            {
                entityWeapons.SelectPreviousWeapon();
            }
            for (int i = 0; i < selectWeaponInputs.Length; i++)
            {
                if (selectWeaponInputs[i].action.WasPressedThisFrame())
                {
                    entityWeapons.SetCurrentWeapon(i);
                }
            }
        }
    }
    void BatWeapon()
    {
        if (meleeAttack.action.WasPerformedThisFrame() && batCharges > 0 && !alreadyAttacking)
        {
            alreadyAttacking = true;
            batCharges--;
            chargeUI.fillAmount -= 1f / 3f;
            fillAmountBat -= 1f / 3f;
            animator.SetTrigger("MeleeAttack");
            if (entityWeapons.GetCurrentWeapon().weaponNumber != 4)
            {
                weaponBeforeBat = entityWeapons.GetCurrentWeapon().weaponNumber;
            }
            entityWeapons.SetCurrentWeapon(4);
            if (!alreadyRecharging)
            {
                StartCoroutine(Recharge());
            }
            else if (batCharges < 3)
            {
                batReloadQueue++;
            }
        }
    }
    void CurrentCDAmount()
    {
        if(fillAmountBat < 1)
        {
            fillAmountBat += (1f / 12f) * Time.deltaTime;
            chargeUI.fillAmount = fillAmountBat;
        }
        switch (batCharges)
        {
            case 0:
                chargesReadyUI[0].SetActive(false);
                chargesReadyUI[1].SetActive(false);
                chargesReadyUI[2].SetActive(false);
                break;
            case 1:
                chargesReadyUI[0].SetActive(false);
                chargesReadyUI[1].SetActive(false);
                chargesReadyUI[2].SetActive(true);
                break;
            case 2:
                chargesReadyUI[0].SetActive(false);
                chargesReadyUI[1].SetActive(true);
                chargesReadyUI[2].SetActive(true);
                break;
            case 3:
                chargesReadyUI[0].SetActive(true);
                chargesReadyUI[1].SetActive(true);
                chargesReadyUI[2].SetActive(true);
                break;
        }
    }
    void PickWeapon()
    {

    }
    public IEnumerator Recharge()
    {
        alreadyRecharging = true;
        yield return new WaitForSeconds(4);
        alreadyRecharging = false;
        if (batCharges < 3)
        {
            batCharges++;
        }
        if (batReloadQueue > 0)
        {
            batReloadQueue--;
            StartCoroutine(Recharge());
        } 
    }
    Vector3 lastVelocity = Vector3.zero;
    private void UpdateMovementOnPlane()
    {
        Vector2 rawMoveValue = move.action.ReadValue<Vector2>();
        Vector3 xzMoveValue = (Vector3.right * rawMoveValue.x) + (Vector3.forward * rawMoveValue.y);

        switch (movementMode)
        {
            case MovementMode.RelativeToCharacter: UpdateMovementRelativeToCharacter(xzMoveValue); break;
            case MovementMode.RelativeToCamera: UpdateMovementRelativeToCamera(xzMoveValue); break;
        }

        void UpdateMovementRelativeToCharacter(Vector3 xzMoveValue)
        {
            Vector3 velocity = xzMoveValue * planeSpeed;
            velocityToApply += velocity;
        }
        void UpdateMovementRelativeToCamera(Vector3 xzMoveValue)
        {
            Transform cameraTransform = Camera.main.transform;
            Vector3 xzMoveValueFromCamera = cameraTransform.TransformDirection(xzMoveValue);
            float originalMagnitude = xzMoveValueFromCamera.magnitude;
            xzMoveValueFromCamera = Vector3.ProjectOnPlane(xzMoveValueFromCamera, Vector3.up).normalized * originalMagnitude;

            Vector3 velocity = xzMoveValueFromCamera * planeSpeed;

            velocityToApply += velocity;
        }
    }
    private void UpdateVerticalMovement()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = 0f;
        }
        verticalVelocity += gravity * Time.deltaTime;

        bool mustJump = jump.action.WasPerformedThisFrame();
        if (mustJump && characterController.isGrounded)
        {
            verticalVelocity = jumpSpeed;
        }

        velocityToApply += verticalVelocity * Vector3.up;
    }
    private void UpdateOrientation()
    {
        Vector3 desiredDirection = Vector3.zero;
        switch (orientationMode)
        {
            case OrientationMode.OrientateToCameraForward:
                desiredDirection = Camera.main.transform.forward;
                break;
            case OrientationMode.OrientateToMovementForward:
                if (velocityToApply.sqrMagnitude > 0f)
                {
                    desiredDirection = lastVelocity;
                }
                break;
            case OrientationMode.OrientateToTarget:
                desiredDirection = orientationTarget.transform.position - transform.position;
                desiredDirection.y = 0;
                break;
        }

        float angularDistance = Vector3.SignedAngle(transform.forward, desiredDirection, Vector3.up);
        float angleToApply = angularSpeed * Time.deltaTime;

        angleToApply = Mathf.Min(angleToApply, Mathf.Abs(angularDistance));
        angleToApply *= Mathf.Sign(angularDistance);

        Quaternion rotationToApply = Quaternion.AngleAxis(angleToApply, Vector3.up);
        transform.rotation = rotationToApply * transform.rotation;
    }
    

    private void OnDisable()
    {
        move.action.Disable();
        jump.action.Disable();
        meleeAttack.action.Disable();
        fire.action.Disable();
        swapWeapon.action.Disable();
        foreach (InputActionReference iar in selectWeaponInputs)
        {
            iar.action.Disable();
        }
    }
    #region IEntityAnimable implementation
    public Vector3 GetLastVelocity()
    {
        return velocityToApply;
    }
    public float GetVerticalVelocity()
    {
        return verticalVelocity;
    }
    public float GetJumpSpeed()
    {
        return jumpSpeed;
    }
    public bool IsGrounded()
    {
        return characterController.isGrounded;
    }
    #endregion
}