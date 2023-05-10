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

    //[SerializeField]
    //WeaponType weaponType = WeaponType.Enemy;

    [SerializeField]
    LayerMask allLayer;
    [SerializeField]
    LayerMask geometryLayer;
    
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
            UIManager.instance.UpdateAmmo(cGunStats.currentClipAmmo, 
            cGunStats.clipSize, 
            inventoryManager.ammoInInventory[cGunStats.ammo.GetAmmoName()]);
        }

        if (recoveryTime <= 0f)
        {
            isReloading = false;
        }

        if (IsLaserActive == true)
        {
            UpdateLaserPointer();
        }

        if ((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton2)) && cGunStats.currentClipAmmo != cGunStats.clipSize)
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
        if (GOPoolScript.instance.poolDictionary.ContainsKey(cGunStats.poolTag))
        {
            projectilePool = GOPoolScript.instance.poolDictionary[cGunStats.poolTag];
            HitScanLineShootVFX VFX = projectilePool.Dequeue().GetComponent<HitScanLineShootVFX>();
            if (VFX == null)
            {
                Debug.LogWarning("there is no projectileScript inside this gameobject");
            }
        }
        else
        {
            Debug.LogWarning("no prefab exist with the tag " + cGunStats.poolTag);
        }
    }

    private void Reload()
    {

        if (inventoryManager.ammoInInventory[cGunStats.ammo.GetAmmoName()] <= 0)
        {
            return;
        }
        
        int ammoDifference = cGunStats.clipSize - cGunStats.currentClipAmmo;
        if (inventoryManager.ammoInInventory[cGunStats.ammo.GetAmmoName()] >= ammoDifference)
        {
            cGunStats.currentClipAmmo = cGunStats.clipSize;
            inventoryManager.ammoInInventory[cGunStats.ammo.GetAmmoName()] -= ammoDifference;
        }
        else
        {
            cGunStats.currentClipAmmo += inventoryManager.ammoInInventory[cGunStats.ammo.GetAmmoName()];
            inventoryManager.ammoInInventory[cGunStats.ammo.GetAmmoName()] = 0;
        }
        isReloading = true;
        recoveryTime = cGunStats.reloadTime;
    }

    public void Shoot()
    {
        RaycastHit2D hit;

        if (cGunStats.currentClipAmmo == 0)
        {
            UIManager.instance.UpdateAmmo(cGunStats.currentClipAmmo,
                cGunStats.clipSize,
                inventoryManager.ammoInInventory[cGunStats.ammo.GetAmmoName()]);

            Reload();
        }

        if (cGunStats.currentClipAmmo > 0 && recoveryTime <= 0f)
        {
            cGunStats.currentClipAmmo--;
            recoveryTime = 1f / cGunStats.rateOfFire; 
            

            gunAiudioSource.PlayOneShot(cGunStats.audioClips[UnityEngine.Random.Range(0, cGunStats.audioClips.Length)]);
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

                pS.InitializeProjcetileHitscan(gunMuzzle.position, hit.point, hit.normal, true);

                //deal damage if it can receive it
                IHealth ihealt = hit.transform.GetComponent<IHealth>();
                if (ihealt != null)
                {
                    ihealt.TakeDamage(cGunStats.damage);
                }
            }
            else
            {
                pS.InitializeProjcetileHitscan(gunMuzzle.transform.position, gunMuzzle.position + gunMuzzle.up * cGunStats.range,-gunMuzzle.forward, false);
            }
        }

        

    }

    public void updateAmmoType(ScriptableGunStats scriptableGun)
    {
        if (!inventoryManager.ammoInInventory.ContainsKey(scriptableGun.ammo.GetAmmoName()))
        {
            inventoryManager.ammoInInventory.Add(scriptableGun.ammo.GetAmmoName(), scriptableGun.ammo.StartingAmmo); ;
        }

    }

}
