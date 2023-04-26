using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Item : MonoBehaviour
{
    [Tooltip("the GameObject that should be spawned if dropped")]
    public GameObject GOReference;
    //[Tooltip("the GameObject that is displayed in the item reference")]
    //public GameObject UIReference;
    public string itemName = "New item";
    public string description = "Look its an item in my inventory";
    public int itemCount = 1;

    public TextMeshProUGUI textUIName;
    public TextMeshProUGUI textUIDescritpion;
    public TextMeshProUGUI textUICount;
    public Image imageUIDispay;
    public Sprite image;

    //[HideInInspector]
    public int itemID;

    protected void Awake()
    {
        textUIName.text = itemName;
        //textUIDescritpion.text = description;
        textUICount.text = itemCount.ToString();
        imageUIDispay.sprite = image;
    }

    public void Equip()
    {

    }
}
