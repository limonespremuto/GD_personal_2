using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerScript : MonoBehaviour
{
    [SerializeField]
    protected Transform targetTransform;
    protected NavMeshAgent agent;

    [SerializeField, Tooltip("are where this enemy can hear the player, currently it just need to be whitin")]
    protected float agroRange;
    protected float distanceToTarget;

    [SerializeField]
    protected LayerMask whatIsWorld;
    [SerializeField]
    protected LayerMask entityLayer;

    // Start is called before the first frame update
    protected void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    protected void Update()
    {
        distanceToTarget = Vector2.Distance(transform.position, targetTransform.position);
        if (distanceToTarget <= agroRange)
            agent.SetDestination(targetTransform.position);
        
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,agroRange);
        
    }
}
