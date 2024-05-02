using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityWeaponsChallenge : EntityWeapons
{
    [SerializeField] GameObject[] weaponsImage;
    [SerializeField] TextMeshProUGUI ammoText;

    [SerializeField] GameObject[] weaponsPickUps;
    [SerializeField] bool[] weaponsTypeAlreadySpawned;
    public bool nextEnemyDrops;
    bool weaponAlreadyDropped;
    int weaponDropCounter;

    private void Start()
    {
        weaponDropCounter = weaponsPickUps.Length;
    }
    public override void SetCurrentWeapon(int selectedWeapon)
    {
        base.SetCurrentWeapon(selectedWeapon);
        for (int i = 0; i < weaponsImage.Length; i++)
        {
            if (i == selectedWeapon)
            {
                weaponsImage[i].SetActive(true);
            }
            else
            {
                weaponsImage[i].SetActive(false);
            }
        }
        if(selectedWeapon == 0)
        {
            ammoText.text = "∞";
            ammoText.fontSize = 75;
        }
        else
        {
            ammoText.text = weapons[selectedWeapon].currentAmmo.ToString();
            ammoText.fontSize = 45;
        }
    }
    private void Update()
    {
        if(currentWeapon != 0)
        {
            ammoText.text = weapons[currentWeapon].currentAmmo.ToString();
        }
        if ((weapons[currentWeapon].currentAmmo <= weapons[currentWeapon].initialAmmo / 5 || currentWeapon == 0) && !weaponAlreadyDropped)
        {
            nextEnemyDrops = true;
        }
        else
        {
            nextEnemyDrops = false;
        }
        if (weapons[currentWeapon].currentAmmo <= 0)
        {
            SetCurrentWeapon(0);
        }
    }
    public void SpawnNewWeapon(Vector3 position)
    {
        weaponAlreadyDropped = true;
        nextEnemyDrops = false;
        int randomWeapon = Random.Range(0, weaponsPickUps.Length);
        while (weaponsTypeAlreadySpawned[randomWeapon])
        {
            randomWeapon = Random.Range(0, weaponsPickUps.Length);
        }
        weaponDropCounter--;
        weaponsTypeAlreadySpawned[randomWeapon] = true;
        GameObject weaponDrop = Instantiate(weaponsPickUps[randomWeapon], new Vector3(position.x, 0, position.z), Quaternion.identity);
        weaponDrop.GetComponent<PickUpWeapon>().dismiss.AddListener(WeaponDismissed);
        if (weaponDropCounter <= 0)
        {
            weaponDropCounter = weaponsPickUps.Length;
            for (int i = 0; i < weaponsTypeAlreadySpawned.Length; i++)
            {
                weaponsTypeAlreadySpawned[i] = false;
            }
        }

    }
    void WeaponDismissed()
    {
        weaponAlreadyDropped = false;
    }
}