using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRanged : AIBase
{
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private float projectileRange = 12f;
    [SerializeField]
    private float projectileSpeed = 3f;
    [SerializeField]
    private float attackRecoveryTime;
    private float attackCooldown;

    [SerializeField]
    private float attackOfset;
    [SerializeField]
    private string attackPrefabPoolTag;
    private Queue<GameObject> attackPrefabPool;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        attackPrefabPool = GOPoolScript.instance.poolDictionary[attackPrefabPoolTag];
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (attackCooldown <= 0)
        {
            if (Vector2.Distance(transform.position, targetTransform.position) <= stopDistance)
            {
                
                GameObject attackPrefab = attackPrefabPool.Dequeue();
                attackPrefab.transform.position = transform.position + (transform.up * attackOfset);
                attackPrefab.SetActive(true);
                attackPrefab.transform.rotation = transform.rotation;

                ProjectileScript pS = attackPrefab.GetComponent<ProjectileScript>();
                if (pS != null)
                {
                    pS.damage = attackDamage;
                    pS.speed = projectileSpeed;
                    pS.activeTime = projectileRange / projectileSpeed;
                    //pS.layerCheck = entityLayer | worldLayer;

                }
                else
                {
                    Debug.LogWarning(attackPrefabPoolTag + " does not contain a ProjectileScript script");
                }
                attackPrefabPool.Enqueue(attackPrefab);
                attackCooldown = attackRecoveryTime;
            }
        }
        attackCooldown -= Time.deltaTime;
    }
}
