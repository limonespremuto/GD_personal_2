using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : PickupAmmo
{
    [SerializeField]
    int AddedGunID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckForPlayer(collision))
        {
            InventoryManager.instace.inventorySO.guns[ammoID].IsAvailable = true;
            InventoryManager.instace.inventorySO.guns[ammoID].reserveAmmo += addedAmmo;
        }
        Destroy(gameObject);
    }
}
