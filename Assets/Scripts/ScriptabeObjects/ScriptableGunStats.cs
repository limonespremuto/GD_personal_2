using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunStats", menuName = "ScriptableObjects/Items/NewWeapon", order = 1)]
public class ScriptableGunStats : ScriptableObject
{
    //public string weaponName = "unnamed wepaon";
    //public string weaponDescription = "no description";
    //public Sprite weaponImage;

    public float damage = 5f;
    public float range = 10f;
    public float rateOfFire = 2f;
    public float recoil = 0f;

    //public InventoryManager.Ammo ammoType;
    //public string ammoTypeName = "default";
    //[Range(0, 999)]
    //public int StartingAmmo = 0;
    public int clipSize = 30;
    public int currentClipAmmo = 0;
    public InventorySO.Ammo ammo;


    public float reloadTime = 2f;


    public AudioClip[] audioClips;
    [Tooltip("Tag of the pooled ProjectielEffect")]
    public string poolTag;
}
