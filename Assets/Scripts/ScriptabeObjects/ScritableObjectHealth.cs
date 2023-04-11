using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealtValue", menuName = "ScriptableObjects/Enity/Hleath", order = 1)]
public class ScritableObjectHealth : ScriptableObject
{
    public float maxHealth = 10f;

    [Range(0f,1f)]
    public float resistance = 0f;
}
