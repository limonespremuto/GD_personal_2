using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{

    public GameObject[] ObjectsToActivate;
    public GameObject[] ObjectsToDisable;
    //[SerializeField]
    //private bool _playerInside = false;
    
    public virtual void Interact()
    {
        foreach (GameObject go in ObjectsToActivate)
        {
            go.SetActive(true);
        }

        foreach (GameObject go in ObjectsToDisable)
        {
            go.SetActive(false);
        }
    }
}
