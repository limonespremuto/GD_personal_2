using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDealDamage : Button
{
    [SerializeField]
    HealtScipt[] healtScriptToDamage;
    public float damage;
    public override void Interact()
    {
        //Debug.Log("beaking sounds");
        base.Interact();
        foreach (HealtScipt h in healtScriptToDamage)
        {
            h.TakeDamage(damage);
        }
    }
}
