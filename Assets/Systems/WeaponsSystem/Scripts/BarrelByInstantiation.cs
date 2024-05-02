using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelByInstantiation : Barrel
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] bool visualRocket;
    RocketReload rocketReload;
    AudioSource audioSource;
    [SerializeField] bool isChallenge;
    Weapon weapon;

    private void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
        audioSource = GetComponent<AudioSource>();
        rocketReload = GetComponent<RocketReload>();
    }
    public override void Shot()
    {
        if(visualRocket)
        {
            if (rocketReload.reloaded)
            {
                if (isChallenge)
                {
                    weapon.currentAmmo--;
                }
                audioSource.Play();
                rocketReload.Reloading();
                Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            }         
        }
        else
        {
            audioSource.Play();
            Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        }
    }
}
