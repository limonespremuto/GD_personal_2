using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string dungeon1Name;
    public InventorySO inventory;
    public int startingAmmo;
    public List<GameObject> startingItems;
    public bool deleteIventory = true;

    public void ToDungeon()
    {
        SceneManager.LoadScene(dungeon1Name);

        if (deleteIventory)
        {
            foreach (ScriptableGunStats Gun in inventory.guns)
            {
                Gun.reserveAmmo = 0;
                Gun.currentClipAmmo = 0;
                Gun.IsAvailable = false;
            }
            inventory.guns[0].reserveAmmo = startingAmmo;
            inventory.guns[0].IsAvailable = true;
            inventory.medkits = 0;
            inventory.cash = 0;
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
