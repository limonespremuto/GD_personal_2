using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instace;
    public InventorySO inventorySO;
    [SerializeField]
    Transform InventoryUITransform;
    [SerializeField]
    private TextMeshProUGUI ammoTMPText;
    public Dictionary<string, int> ammoInInventory;


    private void Awake()
    {
        instace = this;
        ammoInInventory = new Dictionary<string, int>();
        //ammoInInventory = new Dictionary<string, int>();
        foreach (InventorySO.Ammo ammo in inventorySO.ammos)
        {
            ammoInInventory.Add(ammo.GetAmmoName(), ammo.StartingAmmo);
            //Debug.Log(ammoInInventory.Count);
        }
    }

    public void UpdateDisplayedItems()
    {
        int itemID = 0;

        for (int i = 0; i < InventoryUITransform.transform.childCount; i++)
        {
            Destroy(InventoryUITransform.transform.GetChild(i).gameObject);
        }



        foreach (GameObject item in inventorySO.Items)
        {
            GameObject uiItem = Instantiate(item);
            uiItem.transform.SetParent(InventoryUITransform);


            Item itemSc= uiItem.GetComponent<Item>();
            itemSc.itemID = itemID;
            itemID++;
        }
        string AmmoText = "";
        foreach (var valuePair in ammoInInventory)
        {
            AmmoText = AmmoText + valuePair.Key + " [" + valuePair.Value + "] ";
        }
        ammoTMPText.text = AmmoText;
    }

    public void removeItemByID(int ID)
    {
        inventorySO.Items.RemoveAt(ID);
        UpdateDisplayedItems();
        
    }
}
