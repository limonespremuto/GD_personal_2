using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMelee : EnemyControllerScript
{
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private float attackRecoveryTime;
    private float attackCooldown;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (attackCooldown <= 0)
        {
            if (Physics2D.OverlapCircle(transform.position, attackRange, entityLayer))
            {
                Collider2D[] Colliders;
                Colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, entityLayer);
                foreach (Collider2D collider in Colliders)
                {
                    IHealth tHealth = collider.GetComponent<IHealth>();
                    if (tHealth != null)
                    {
                        tHealth.TakeDamage(attackDamage);
                    }
                }
                attackCooldown = attackRecoveryTime;
            }

        }

        attackCooldown -= Time.deltaTime;
    }

    new private void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
