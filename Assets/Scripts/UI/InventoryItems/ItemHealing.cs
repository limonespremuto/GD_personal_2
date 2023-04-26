using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealing : Item
{

    public float HealAmmount;
    
    new public void Equip()
    {
        PlayerController.instance.healt = Mathf.Clamp(PlayerController.instance.healt + HealAmmount, 0 , PlayerController.instance.maxHealt);

        UIManager.instance.UpdateHealth(PlayerController.instance.healt, PlayerController.instance.maxHealt);
        InventoryManager.instace.removeItemByID(itemID);
        Destroy(gameObject);
    }
}
