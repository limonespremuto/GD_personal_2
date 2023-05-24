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


    public TextMeshProUGUI medkitsText;
    public TextMeshProUGUI cashText;

    //[Tooltip("its ID in this list is what changes it.")]
    public Guns[] uiGuns;

    private WeaponScript _playerWeaponScript;
    
    [System.Serializable]
    public class Guns
    {
        public Transform uiGunTransform;
        public TextMeshProUGUI gunNameField;
        public TextMeshProUGUI ammoField;
    }

    private void Awake()
    {
        instace = this;
    }

    private void Start()
    {
        _playerWeaponScript = PlayerController.instance.weaponScript;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !_playerWeaponScript.isReloading)
        {
            PlayerController.instance.weaponScript.cGunStats = inventorySO.guns[0];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !_playerWeaponScript.isReloading)
        {
            if (inventorySO.guns.Length - 1 >= 1)
                PlayerController.instance.weaponScript.cGunStats = inventorySO.guns[1];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !_playerWeaponScript.isReloading)
        {
            if (inventorySO.guns.Length -1 >= 2)
                PlayerController.instance.weaponScript.cGunStats = inventorySO.guns[2];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && !_playerWeaponScript.isReloading)
        {
            if (inventorySO.guns.Length - 1 >= 3)
                PlayerController.instance.weaponScript.cGunStats = inventorySO.guns[3];
        }

        UpdateWeaponDisplay();
    }

    public void UpdateWeaponDisplay()
    {
        int AmmoToDisplay;
        for (int i = 0; i < inventorySO.guns.Length; i++)
        {
            AmmoToDisplay = inventorySO.guns[i].reserveAmmo + inventorySO.guns[i].currentClipAmmo;
            uiGuns[i].gunNameField.text = inventorySO.guns[i].name;
            uiGuns[i].ammoField.text = " " + AmmoToDisplay;
            uiGuns[i].uiGunTransform.gameObject.SetActive(inventorySO.guns[i].IsAvailable);
        }

        cashText.text = "Cash: " + inventorySO.cash;
        medkitsText.text = "Medkits: " + inventorySO.medkits;
    }

    /*
    public void removeItemByID(int ID)
    {
        inventorySO.Items.RemoveAt(ID);
        UpdateDisplayedItems();
        
    }
    */
}
