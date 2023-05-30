using System;
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
    protected float DetectionRange = 10f;
    [SerializeField]
    protected float stopDistance = 1f;
    protected float distanceToTarget;
    [SerializeField]
    protected float speed = 2f;

    [SerializeField]
    protected LayerMask worldLayer;
    [SerializeField]
    protected LayerMask entityLayer;
    public ETeam myTeam = ETeam.Object;

    [Tooltip("how much time before AI updates target")]
    public float checkTime = 1f;
    protected float _CheckTimeCooldown = 0f;

    public enum ETeam
    {
        Object,
        Zombies,
        Soldiers
    }

    // Start is called before the first frame update
    protected void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        _CheckTimeCooldown += UnityEngine.Random.Range(0f, checkTime);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (targetTransform != null)
        {
            distanceToTarget = Vector2.Distance(transform.position, targetTransform.position);
            //Debug.Log(distanceToTarget);

            bool hasSight = CheckSight(targetTransform.position, worldLayer);    

            if (distanceToTarget <= DetectionRange && agent.isActiveAndEnabled && hasSight)
            {
                agent.SetDestination(targetTransform.position);
            }
        }

        if (_CheckTimeCooldown <= 0)
        {
            FindEnemyInArea(DetectionRange, entityLayer);
            _CheckTimeCooldown = checkTime;
        }

        _CheckTimeCooldown -= Time.deltaTime;
    }
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);

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
        Vector2 end = new Vector2(target.x - transform.position.x , target.y - transform.position.y);
        float distance = Vector2.Distance(transform.position, target);

        if (!Physics2D.Raycast(start, end, distance, layermask))
        {
            Debug.DrawLine(start, (Vector2)target, Color.green);
            return true;
        }

        Debug.DrawLine(start, (Vector2)target, Color.red);
        return false;
    }

    public void FindEnemyInArea( float range, LayerMask enemyTeamsLayer)
    {
        Collider2D[] foundTargets = Physics2D.OverlapCircleAll(transform.position, range, enemyTeamsLayer);

        

        HashSet<Transform> targets = new HashSet<Transform>();

        foreach (Collider2D foundTarget in foundTargets)
        {
            if (CheckSight(foundTarget.transform.position, worldLayer))
            {
                AIBase targetToAdd = foundTarget.GetComponentInParent<AIBase>();
                if (targetToAdd != null)
                {
                    if (targetToAdd.myTeam != myTeam)
                    {
                        targets.Add(targetToAdd.transform);
                    }
                }
            
                if (foundTarget.GetComponentInParent<PlayerController>() != null)
                {
                    targets.Add(foundTarget.transform.root);
                }
            }
        }

        List<Transform> trans = new List<Transform>();
        foreach (Transform t in targets)
        {
            trans.Add(t);
        }

        Transform[] transforms = trans.ToArray();
        Array.Sort(transforms,new DistanceComparer(transform));
        if (transforms.Length != 0)
        {
            targetTransform = transforms[0];
        }
    }
}
