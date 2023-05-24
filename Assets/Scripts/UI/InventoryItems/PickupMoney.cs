using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMoney : PickUp
{
    [SerializeField]
    int AddedMoney;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckForPlayer(collision))
        {
            InventoryManager.instace.inventorySO.cash += AddedMoney;
        }
        Destroy(gameObject);
    }
}
