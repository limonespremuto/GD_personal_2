using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunStats", menuName = "ScriptableObjects/Items/NewWeapon", order = 1)]
public class ScriptableGunStats : ScriptableObject
{
    public string weaponName = "unnamed wepaon";
    public string weaponDescription = "no description";
    public Sprite weaponImage;

    public float damage = 5f;
    public float range = 10f;
    public float rateOfFire = 2f;
    public float recoil = 0f;

    public float maxAmmo = 90f;
    public float clipAmmo = 30f;
}
