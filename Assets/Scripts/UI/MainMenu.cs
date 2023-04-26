using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string dungeon1Name;
    public InventorySO inventory;
    public InventorySO.Ammo[] startingAmmo;
    public List<GameObject> startingItems;
    public bool deleteIventory = true;

    public void ToDungeon()
    {
        SceneManager.LoadScene(dungeon1Name);

        if (deleteIventory)
        {
            inventory.Items = new List<GameObject>();
            inventory.Items = startingItems;

            inventory.ammoInInventory = new Dictionary<string, int>();
            foreach (InventorySO.Ammo ammo in startingAmmo)
            {
                inventory.ammoInInventory.Add(ammo.GetAmmoName(), ammo.StartingAmmo);
                //Debug.Log(ammoInInventory.Count);
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
        
    }

    public void changeKeepInventory(bool keepInventory)
    {
        deleteIventory = !keepInventory;
    }
}
