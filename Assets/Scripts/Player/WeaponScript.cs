using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField]
    public ScriptableGunStats cGunStats; 

    float recoveryTime;
    bool isReloading = false;
    int currentClipAmmo;

    private InventoryManager inventoryManager;

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
    AudioSource gunAiudioSource;
    [SerializeField]
    AudioClip[] audioClips;


    //[SerializeField]
    //WeaponType weaponType = WeaponType.Enemy;

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
        GetPool();
        inventoryManager = InventoryManager.instace;
        updateAmmoType(cGunStats);
    }

   

    // Update is called once per frame
    void Update()
    {
        if (!isReloading)
        {
            UIManager.instance.UpdateAmmo(currentClipAmmo, 
                cGunStats.clipSize, 
                inventoryManager.ammoInInventory[cGunStats.ammoTypeName]);
        }
        if (isReloading && recoveryTime <= 0f)
        {
            isReloading = false;
        }

        if (IsLaserActive == true)
        {
            UpdateLaserPointer();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentClipAmmo != cGunStats.clipSize)
        {
            Reload();
        }
        recoveryTime -= Time.deltaTime;
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

    private void GetPool()
    {
        if (GOPoolScript.instance.poolDictionary.ContainsKey(poolTag))
        {
            projectilePool = GOPoolScript.instance.poolDictionary[poolTag];
            HitScanLineShootVFX VFX = projectilePool.Dequeue().GetComponent<HitScanLineShootVFX>();
            if (VFX == null)
            {
                Debug.LogWarning("there is no projectileScript inside this gameobject");
            }
        }
        else
        {
            Debug.LogWarning("no prefab exist with the tag " + poolTag);
        }
    }

    private void Reload()
    {

        if (inventoryManager.ammoInInventory[cGunStats.ammoTypeName] <= 0)
        {
            return;
        }
        
        int ammoDifference = cGunStats.clipSize - currentClipAmmo;
        if (inventoryManager.ammoInInventory[cGunStats.ammoTypeName] >= ammoDifference)
        {
            currentClipAmmo = cGunStats.clipSize;
            inventoryManager.ammoInInventory[cGunStats.ammoTypeName] -= ammoDifference;
        }
        else
        {
            currentClipAmmo += inventoryManager.ammoInInventory[cGunStats.ammoTypeName];
            inventoryManager.ammoInInventory[cGunStats.ammoTypeName] = 0;
        }
        isReloading = true;
        recoveryTime = cGunStats.reloadTime;
    }

    public void Shoot(bool isEnemyFiring)
    {
        RaycastHit2D hit;

        if (currentClipAmmo == 0)
        {
            UIManager.instance.UpdateAmmo(currentClipAmmo,
                cGunStats.clipSize,
                inventoryManager.ammoInInventory[cGunStats.ammoTypeName]);

            Reload();
        }

        if (currentClipAmmo > 0 && recoveryTime <= 0f)
        {
            currentClipAmmo--;
            recoveryTime = 1f / cGunStats.rateOfFire; 
            

            gunAiudioSource.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length)]);
            //take from pool and enable it
            GameObject projectileGO = projectilePool.Dequeue();
            projectilePool.Enqueue(projectileGO);
            projectileGO.transform.position = gunMuzzle.transform.position;
            projectileGO.transform.rotation = gunMuzzle.transform.rotation;
            
            HitScanLineShootVFX pS = projectileGO.GetComponent<HitScanLineShootVFX>();
            if (pS == null)
            {
                Debug.LogWarning("there is no projectileScript inside this gameobject");
            }
            projectileGO.SetActive(true);
        
            if (hit = Physics2D.Raycast(gunMuzzle.position, gunMuzzle.up, cGunStats.range, allLayer))
            {
                //Debug.Log("pew");

                pS.InitializeProjcetileHitscan(gunMuzzle.position, hit.point, hit.normal);

                //deal damage if it can receive it
                IHealth ihealt = hit.transform.GetComponent<IHealth>();
                if (ihealt != null)
                {
                    ihealt.TakeDamage(cGunStats.damage);
                }
            }
            else
            {
                pS.InitializeProjcetileHitscan(gunMuzzle.transform.position, gunMuzzle.position + gunMuzzle.up * cGunStats.range,-gunMuzzle.forward);
            }
        }

        

    }

    public void updateAmmoType(ScriptableGunStats scriptableGun)
    {
        if (!inventoryManager.ammoInInventory.ContainsKey(scriptableGun.ammoTypeName))
        {
            inventoryManager.ammoInInventory.Add(scriptableGun.ammoTypeName, scriptableGun.StartingAmmo);
        }

    }

}
