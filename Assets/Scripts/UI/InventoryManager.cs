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

    [SerializeField]
    private Ammo[] ammos;
    public Dictionary<string, int> ammoInInventory;

    [System.Serializable]
    public class Ammo
    {
        public string ammoTypeName = "default";
        [Range(0,999)]
        public int StartingAmmo = 0;
    }

    private void Awake()
    {
        instace = this;
        ammoInInventory = new Dictionary<string, int>();
        foreach (Ammo ammo in ammos)
        {
            ammoInInventory.Add(ammo.ammoTypeName, ammo.StartingAmmo);
            //Debug.Log(ammoInInventory.Count);
        }
    }

    public void UpdateDisplayedItems()
    {
        for (int i = 0; i < InventoryUITransform.transform.childCount; i++)
        {
            Destroy(InventoryUITransform.transform.GetChild(i).gameObject);
        }

        foreach (GameObject item in inventorySO.Items)
        {
            GameObject uiItem = Instantiate(item);
            uiItem.transform.SetParent(InventoryUITransform);
        }
        string AmmoText = "";
        foreach (var valuePair in ammoInInventory)
        {
            AmmoText = AmmoText + valuePair.Key + " [" + valuePair.Value + "] ";
        }
        ammoTMPText.text = AmmoText;
    }
}
