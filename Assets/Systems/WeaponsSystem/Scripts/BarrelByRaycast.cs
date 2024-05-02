using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BarrelByRaycast : Barrel
{
    [Header("Weapon Parts")]
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform shellEjectPoint;
    [SerializeField] GameObject shell;
    [SerializeField] GameObject tracerPrefab;
    VisualEffect muzzleFlash;

    [Header("Weapon Stats")]
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 1;
    [SerializeField] float cadence = 15f;
    [SerializeField] Vector2 dispersionAngles = new Vector2(5, 5);
    [SerializeField] bool isChallenge;
    [SerializeField] bool doesWallbang;
    [SerializeField] int burstDuration;
    int currentBurst;
    Weapon weapon;

    [Header("Other")]
    [SerializeField] LayerMask layerMask = Physics.DefaultRaycastLayers;

    bool isContinuousShooting;
    bool isBurstMode;
    bool shootCoolDown;
    bool drawing;
    float nextShotTime;
    private void OnEnable()
    {
        currentBurst = burstDuration;
        StopShooting();
        StartCoroutine(DrawTime());
        if (muzzleFlash)
        {
            foreach (Transform child in muzzleFlash.transform)
            {
                child.gameObject.SetActive(false);
            }
            muzzleFlash.Stop();
        }
    }
    private void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
        muzzleFlash = GetComponentInChildren<VisualEffect>();
    }
    private void Update()
    {
        if (Time.time > nextShotTime)           
        {
            shootCoolDown = false;
            if (isContinuousShooting)
            {
                Shot();
            }
            else if (isBurstMode)
            {
                Shot();
                currentBurst--;
                if(currentBurst == 0)
                {
                    currentBurst = burstDuration;
                    isBurstMode = false;
                }
            }
        }
    }
    public override void Shot()
    {
        if(!shootCoolDown && !drawing)
        {
            if (isChallenge)
            {
                weapon.currentAmmo--;
            }
            nextShotTime = Time.time;
            nextShotTime += 1f / cadence;
            shootCoolDown = true;
            if (muzzleFlash)
            {
                foreach (Transform child in muzzleFlash.transform)
                {
                    child.gameObject.SetActive(true);
                }
                muzzleFlash.Play();
            }
            Vector3 dispersedForward = DispersedForward();
            Vector3 finalPosition = transform.position + dispersedForward.normalized * range;
            GameObject tracerGO = Instantiate(tracerPrefab);
            if(doesWallbang)
            {
                RaycastHit[] hits = Physics.RaycastAll(shootPoint.position, dispersedForward, range, layerMask);
                if(hits.Length > 0)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        RaycastHit hit = hits[i];
                        hit.collider.GetComponent<Hurtbox>()?.NotifyHit(this, damage);
                        if(i == hits.Length - 1)
                        {
                            finalPosition = hit.point;
                        }
                    }
                }
            }
            else
            {
                if (Physics.Raycast(shootPoint.position, dispersedForward, out RaycastHit hit, range, layerMask))
                {
                    finalPosition = hit.point;
                    hit.collider.GetComponent<Hurtbox>()?.NotifyHit(this, damage);
                }
            }    
            if (shell)
            {
                Instantiate(shell, shellEjectPoint.position, shellEjectPoint.rotation);
            }
            Tracer tracer = tracerGO.GetComponent<Tracer>();
            tracer.Init(shootPoint.position, finalPosition);
            //if(weapon.shotMode == Weapon.ShotMode.burstmode && currentBurst > 0)
            //{
            //    currentBurst--;
            //    Shot();
            //}
            //else if(currentBurst == 0)
            //{
            //    currentBurst = burstDuration;
            //}
        }
    }
    private Vector3 DispersedForward()
    {
        float horizontalDispersionAngle = UnityEngine.Random.Range(-dispersionAngles.x, dispersionAngles.x);
        float verticalDispersionAngle = UnityEngine.Random.Range(-dispersionAngles.y, dispersionAngles.y);
        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalDispersionAngle, transform.up);
        Quaternion verticalRotation = Quaternion.AngleAxis(verticalDispersionAngle, transform.right);
        return horizontalRotation * verticalRotation * transform.forward;
    }
    public override void StartShooting()
    {
        isContinuousShooting = true;
    }
    public override void StopShooting()
    {
        isContinuousShooting = false;
    }
    public override void BurstShooting()
    {
        isBurstMode = true;
    }
    IEnumerator DrawTime()
    {
        drawing = true;
        yield return new WaitForSeconds(0.4f);
        drawing = false;
    }
}