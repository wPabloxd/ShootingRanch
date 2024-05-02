using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDWeaponSelector : MonoBehaviour
{
    [SerializeField] Vector2[] positions;
    [SerializeField] int currentPosition;
    [SerializeField] Image hudImage;
    Image image;
    EntityWeapons entityWeapons;
    private void Awake()
    {
        image = GetComponent<Image>();
        entityWeapons = GetComponentInParent<EntityWeapons>();
    }
    void Start()
    {
        image.rectTransform.localPosition = new Vector2(positions[currentPosition].x, hudImage.rectTransform.localPosition.y - 5.8f);
    }
    void Update()
    {
        currentPosition = entityWeapons.currentWeapon;
        image.rectTransform.localPosition = new Vector2(positions[currentPosition].x, hudImage.rectTransform.localPosition.y - 5.8f);
    }
}