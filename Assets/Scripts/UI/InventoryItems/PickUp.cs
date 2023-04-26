using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    [SerializeField]
    bool addsItem = false;
    public GameObject itemPrefab;

    [SerializeField]
    bool addsAmmo = false;
    
    [SerializeField]
    InventorySO.Ammo ammoType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.transform.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (addsItem)
            {
                InventoryManager.instace.inventorySO.Items.Add(itemPrefab);

            }

            if (addsAmmo)
            {
                if (InventoryManager.instace.ammoInInventory.ContainsKey(ammoType.GetAmmoName()))
                {
                    int newAmmo = InventoryManager.instace.ammoInInventory[ammoType.GetAmmoName()];
                    newAmmo += ammoType.StartingAmmo;
                    InventoryManager.instace.ammoInInventory[ammoType.GetAmmoName()] = newAmmo;
                }
                else
                {
                    InventoryManager.instace.ammoInInventory.Add(ammoType.GetAmmoName(), ammoType.StartingAmmo);
                }

            }
            Destroy(gameObject);
        }
    }
}
