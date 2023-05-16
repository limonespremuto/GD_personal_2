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
    private float attackRange = 8f;

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
        if (targetTransform != null)
        {
            RaycastHit2D CheckHit = Physics2D.Raycast(transform.position, targetTransform.position, distanceToTarget, worldLayer);
            if (attackCooldown <= 0)
            {
                if (distanceToTarget <= attackRange)
                {
                
                    if (CheckHit.collider == null)
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

            }
            if (distanceToTarget <= stopDistance && CheckHit.collider == null)
            {
                transform.up = new Vector3(targetTransform.position.x - transform.position.x, targetTransform.position.y - transform.position.y, 0f);

                agent.enabled = false;
            }
            else
            {
                agent.enabled = true;
            }
            attackCooldown -= Time.deltaTime;

        }
    }
}
