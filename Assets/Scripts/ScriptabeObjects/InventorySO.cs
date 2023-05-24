using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/UI/Inventory", order = 1)]
public class InventorySO : ScriptableObject
{
    public ScriptableGunStats[] guns;
    public int medkits = 0;
    public int cash = 0;
}
