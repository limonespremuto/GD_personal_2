using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{



    protected bool CheckForPlayer(Collider2D collision)
    {
        PlayerController playerController = collision.transform.GetComponent<PlayerController>();
        if (playerController != null)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckForPlayer(collision))
        {
            
        }
    }
}
