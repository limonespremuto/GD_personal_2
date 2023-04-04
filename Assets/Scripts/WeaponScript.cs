using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField]
    ScriptableGunStats cGunStats; 

    float revoceryTime;
    float currentClipAmmo;

    [SerializeField]
    Transform gunHolder;
    [SerializeField]
    Transform gunMuzzle;
    [SerializeField]
    LineRenderer laserPointer;
    [SerializeField]
    bool IsLaserActive;
    //[SerializeField]
    //LineRenderer shootingVfxLine;
    Queue<GameObject> projectilePool;

    [SerializeField]
    WeaponType weaponType = WeaponType.Enemy;

    [SerializeField]
    LayerMask allLayer;
    [SerializeField]
    LayerMask geometryLayer;
    [SerializeField, Tooltip("Tag of the pooled object")]
    string poolTag;

    enum WeaponType
    {
        Player,
        Enemy
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (GOPoolScript.instance.poolDictionary.ContainsKey(poolTag))
        {
            projectilePool = GOPoolScript.instance.poolDictionary[poolTag];
            ProjectileScript pS = projectilePool.Dequeue().GetComponent<ProjectileScript>();
            if (pS != null)
            {
                Debug.LogWarning("there is no projectileScript inside this gameobject");
            }
        }
        else
        {
            Debug.LogWarning("no prefab exist with the tag " + tag);
        }
    }

    private void UpdateLaserPointer()
    {
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(gunMuzzle.position, gunMuzzle.up, cGunStats.range, allLayer))
        {
            //Debug.Log(hit.collider.name);

            laserPointer.SetPosition(0, gunMuzzle.position);
            laserPointer.SetPosition(1, hit.point);

        }
        else
        {
            laserPointer.SetPosition(0, gunMuzzle.position);
            laserPointer.SetPosition(1, gunMuzzle.position + gunMuzzle.up * cGunStats.range);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLaserActive == true)
        {
            UpdateLaserPointer();
        }
    }

    public void Shoot(bool isEnemyFiring)
    {
        RaycastHit2D hit;

        //take from pool and enable it
        GameObject projectileGO = projectilePool.Dequeue();
        projectilePool.Enqueue(projectileGO);
        projectileGO.transform.position = gunMuzzle.transform.position;
        projectileGO.transform.rotation = gunMuzzle.transform.rotation;
            
        ProjectileScript pS = projectileGO.GetComponent<ProjectileScript>();
        if (pS != null)
        {
            Debug.LogWarning("there is no projectileScript inside this gameobject");
        }
        projectileGO.SetActive(true);
        
        if (hit = Physics2D.Raycast(gunMuzzle.position, gunMuzzle.up, cGunStats.range, allLayer))
        {
            Debug.Log("pew");

            pS.InitializeProjcetile(gunMuzzle.transform.position, hit.point, hit.normal);

            //deal damage if it can receive it
            IHealth ihealt = hit.transform.GetComponent<IHealth>();
            if (ihealt != null)
            {
                ihealt.TakeDamage(cGunStats.damage);
            }
        }
        else
        {
            pS.InitializeProjcetile(gunMuzzle.transform.position, gunMuzzle.position + gunMuzzle.up * cGunStats.range,-gunMuzzle.forward);
        }

    }
}
