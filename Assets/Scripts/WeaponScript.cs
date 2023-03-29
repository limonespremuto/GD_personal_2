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
    [SerializeField]
    LineRenderer shootingVfxLine;

    [SerializeField]
    WeaponType weaponType = WeaponType.Enemy;

    [SerializeField]
    LayerMask allLayer;
    [SerializeField]
    LayerMask geometryLayer;

    enum WeaponType
    {
        Player,
        Enemy
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    private void UpdateLaserPointer()
    {
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(gunMuzzle.position, gunMuzzle.up, cGunStats.range, allLayer))
        {
            Debug.Log(hit.collider.name);

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
        
        if (hit = Physics2D.Raycast(gunMuzzle.position, gunMuzzle.up, cGunStats.range, allLayer))
        {
            IHealth ihealt = hit.transform.GetComponent<IHealth>();
            if (ihealt != null)
            {
                ihealt.TakeDamage(cGunStats.damage);

            }
        }

    }
}
