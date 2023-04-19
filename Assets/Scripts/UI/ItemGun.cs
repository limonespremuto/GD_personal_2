using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemGun : Item
{
    public ScriptableGunStats gunStats;
    new protected void Awake()
    {
        base.Awake();
    }

    new public void Equip()
    {
        WeaponScript weaponScript = PlayerController.instance.transform.GetComponentInChildren<WeaponScript>();
        weaponScript.cGunStats = gunStats;
        weaponScript.updateAmmoType(gunStats);

        base.Equip();
    }
}
