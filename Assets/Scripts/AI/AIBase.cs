using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBase : MonoBehaviour
{
    [SerializeField]
    protected Transform targetTransform;
    protected NavMeshAgent agent;

    [SerializeField, Tooltip("are where this enemy can hear the player, currently it just need to be whitin")]
    protected float agroRange = 3f;
    [SerializeField]
    protected float stopDistance = 1f;
    protected float distanceToTarget;
    [SerializeField]
    protected float speed = 2f;

    [SerializeField]
    protected LayerMask worldLayer;
    [SerializeField]
    protected LayerMask entityLayer;

    // Start is called before the first frame update
    protected void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;

    }

    // Update is called once per frame
    protected void Update()
    {
        distanceToTarget = Vector2.Distance(transform.position, targetTransform.position);
        //Debug.Log(distanceToTarget);

        bool hasSight = CheckSight(transform.position, worldLayer);    

        if (distanceToTarget <= agroRange && agent.isActiveAndEnabled && hasSight)
        {
            agent.SetDestination(targetTransform.position);
        }

    }
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, agroRange);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target">the vector 3 is converted to a vector 2</param>
    /// <param name="layermask">what will be taken into account when cheking for line of sight</param>
    /// <returns></returns>
    protected bool CheckSight(Vector3 target, LayerMask layermask)
    {
        Vector2 start = new Vector2(transform.position.x, transform.position.y);
        Vector2 end = new Vector2(target.x, target.y);
        float distance = Vector2.Distance(start, end);

        RaycastHit2D CheckHit = Physics2D.Raycast(start, end, distance, layermask);

        if (CheckHit == false)
        {
            return true;
        }

        return false;
    }
}
