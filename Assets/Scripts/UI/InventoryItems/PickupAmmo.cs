using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : PickUp
{

    [SerializeField]
    protected int ammoID;
    [SerializeField]
    protected int addedAmmo;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckForPlayer(collision))
        {
            InventoryManager.instace.inventorySO.guns[ammoID].reserveAmmo += addedAmmo;
        }
        Destroy(gameObject);
    }
}
