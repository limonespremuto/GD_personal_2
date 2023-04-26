using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMelee : AIBase
{
    [SerializeField]
    private float attackRadius = 0.5f;
    [SerializeField]
    private float attackOfset = 0.5f;
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private float attackRecoveryTime;
    private float attackCooldown;

    [SerializeField]
    private string attackEffectPoolTag;
    private Queue<GameObject> attackEffectPool;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        agent.stoppingDistance = stopDistance;
        attackEffectPool = GOPoolScript.instance.poolDictionary[attackEffectPoolTag];
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (attackCooldown <= 0)
        {
            if (Physics2D.OverlapCircle(transform.position + (transform.up * attackOfset), attackRadius, entityLayer))
            {
                
                Collider2D[] Colliders;
                Colliders = Physics2D.OverlapCircleAll(transform.position + (transform.up * attackOfset), attackRadius, entityLayer);
                foreach (Collider2D collider in Colliders)
                {
                    IHealth tHealth = collider.GetComponent<IHealth>();
                    if (tHealth != null)
                    {
                        tHealth.TakeDamage(attackDamage);
                    }
                }
                attackCooldown = attackRecoveryTime;
                
                
                GameObject attackEffect = attackEffectPool.Dequeue();
                attackEffect.transform.position = transform.position + (transform.up * attackOfset);
                attackEffect.SetActive(true);
                attackEffect.transform.up = transform.up;
                attackEffectPool.Enqueue(attackEffect);
            }

        }

        attackCooldown -= Time.deltaTime;
    }

    new private void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.up * attackOfset), attackRadius);
    }
}
