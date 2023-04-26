using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/UI/Inventory", order = 1)]
public class InventorySO : ScriptableObject
{
    public List<GameObject> Items;

    
    public Ammo[] ammos;
    public Dictionary<string, int> ammoInInventory;

    [System.Serializable]
    public class Ammo
    {
        //public string ammoTypeName = "pistol rounds";
        [Range(0, 999)]
        public int StartingAmmo = 0;
        public AmmoType ammotype;

        public enum AmmoType
        {
            PistolRounds,
            RifleRounds,
            BuckShots
        }

        public string GetAmmoName()
        {
            string AmmoTypeName = null;
            switch (ammotype)
            {
                case AmmoType.PistolRounds:
                    {
                        AmmoTypeName = "pistol rounds";
                        break;
                    }
                case AmmoType.RifleRounds:
                    {
                        AmmoTypeName = "RifleR";
                        break;
                    }
                case AmmoType.BuckShots:
                    {
                        AmmoTypeName = "Buckshots";
                        break;
                    }
                default:
                    {
                        return null;
                    }
            }

            return AmmoTypeName;
        }
    }
}
