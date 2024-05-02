using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RocketReload : MonoBehaviour
{
    [SerializeField] GameObject rocket;
    [SerializeField] VisualEffect muzzleFlash;

    public bool reloaded = true;

    private void OnEnable()
    {
        if (!reloaded)
        {
            StartCoroutine(Reloaded());
        }
        if (muzzleFlash)
        {
            foreach (Transform child in muzzleFlash.transform)
            {
                child.gameObject.SetActive(false);
            }
            muzzleFlash.Stop();
        }
    }
    public void Reloading()
    {
        if (muzzleFlash)
        {
            foreach (Transform child in muzzleFlash.transform)
            {
                child.gameObject.SetActive(true);
            }
            muzzleFlash.Play();
        }
        if (reloaded)
        {
            rocket.SetActive(false);
            reloaded = false;
            StartCoroutine(Reloaded());
        }
    }
    IEnumerator Reloaded()
    {
        yield return new WaitForSeconds(1);
        rocket.SetActive(true);
        reloaded = true;
    }
}
