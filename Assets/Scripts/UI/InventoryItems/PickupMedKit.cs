using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMedKit : PickUp
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckForPlayer(collision))
        {
            InventoryManager.instace.inventorySO.medkits ++;
        }
        Destroy(gameObject);
    }
}
