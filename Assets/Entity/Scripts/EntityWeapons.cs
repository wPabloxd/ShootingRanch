using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWeapons : MonoBehaviour
{
    [Header("Parents")]
    [SerializeField] Transform weaponsParent;
    protected Weapon[] weapons;

    [Header("SoundFXs")]
    [SerializeField] AudioClip[] unholster;

    AudioSource audioSource;
    public int currentWeapon = -1;
    bool initialitated;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        weapons = weaponsParent.GetComponentsInChildren<Weapon>();
        currentWeapon = weapons.Length > 0 ?  0 : -1;
        SetCurrentWeapon(currentWeapon);
    }
    public virtual void SetCurrentWeapon(int selectedWeapon)
    {
        if(selectedWeapon == currentWeapon && initialitated)
        {
            return;
        }
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == selectedWeapon);
        }
        initialitated = true;
        int randomIndex = UnityEngine.Random.Range(0, unholster.Length);
        audioSource.clip = unholster[randomIndex];
        audioSource.Play();
        currentWeapon = selectedWeapon;
    }
    public void SelectNextWeapon()
    {
        int nextWeapon = currentWeapon + 1;
        if (nextWeapon >= weapons.Length - 1)
        {
            nextWeapon = 0;
        }
        SetCurrentWeapon(nextWeapon);
    }
    public void SelectPreviousWeapon()
    {
        int nextWeapon = currentWeapon - 1;
        if (nextWeapon < 0)
        {
            nextWeapon = 3;
        }
        SetCurrentWeapon(nextWeapon);
    }
    public void Shot()
    {
        if(currentWeapon != -1)
        {
            weapons[currentWeapon].Shot();
        }
    }
    public void StartShooting()
    {
        if (currentWeapon != -1)
        {
            weapons[currentWeapon].StartShooting();
        }
    }
    public void StopShooting()
    {
        if (currentWeapon != -1)
        {
            weapons[currentWeapon].StopShooting();
        }
    }
    public void BurstShooting()
    {
        if (currentWeapon != -1)
        {
            weapons[currentWeapon].BurstShooting();
        }
    }
    internal bool HasCurrentWeapon()
    {
        return currentWeapon != -1;
    }
    internal Weapon GetCurrentWeapon()
    {
        return weapons[currentWeapon];
    }
}