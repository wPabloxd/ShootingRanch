using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelByParticles : Barrel
{
    [SerializeField] ParticleSystem particlesSystem;
    [SerializeField] GameObject[] lights;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] bool isChallenge;
    Weapon weapon;

    bool shooting;
    private void OnEnable()
    {
        audioSource.Stop();
        shooting = false;
        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
        particlesSystem.Stop();
    }
    public override void StartShooting()
    {
        weapon = GetComponentInParent<Weapon>();
        audioSource.clip = audioClips[0];
        audioSource.Play();
        shooting = true;
        StartCoroutine(AmmoCounter());
        foreach (GameObject light in lights)
        {
            light.SetActive(true);
        }
        particlesSystem.Play();
    }
    IEnumerator AmmoCounter()
    {
        if (shooting)
        {
            yield return new WaitForSeconds(0.05f);
            weapon.currentAmmo--;
            StartCoroutine(AmmoCounter());
        }
    }
    private void Update()
    {
        if(shooting && !audioSource.isPlaying)
        {
            audioSource.clip = audioClips[1];
            audioSource.Play();
        }
    }
    public override void StopShooting()
    {
        audioSource.clip = audioClips[2];
        audioSource.Play();
        shooting = false;
        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
        particlesSystem.Stop();
    }
}