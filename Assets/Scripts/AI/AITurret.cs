using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurret : MonoBehaviour
{
    public Transform targetTransform;

    public LayerMask worldLayer;
    public LayerMask destructibleLayer;
    public LayerMask enemyLayer;

    [SerializeField]
    Transform _gunTranform;

    [SerializeField]
    private float _detectionAngle;
    [SerializeField]
    private float _detectionDistance;
    [SerializeField]
    private float _shootAngle;
    private float _targetLossTime;
    [SerializeField]
    private float _targetLossDelay;
    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private Transform MuzzleTransform;
    [Tooltip("Time between shoots")]
    public float fireCooldown = 0.1f;
    private float _fireRecoveryDelay = 0f;
    public float attackDamage = 10f;
    public float projectileSpeed = 10f;
    public float projectileRange = 10f;

    [SerializeField]
    AIBase.ETeam myTeam = AIBase.ETeam.Soldiers;

    [SerializeField]
    private GOPoolScript.Pool _shootGOPool = new GOPoolScript.Pool();
    private Queue<GameObject> _attackPrefabPool;

    private void Start()
    {
        targetTransform = PlayerController.instance.transform;
        GOPoolScript.instance.ExtendPools(_shootGOPool);
        _attackPrefabPool = GOPoolScript.instance.poolDictionary[_shootGOPool.tag];
    }

    // Update is called once per frame
    void Update()
    {
        FindEnemyInArea(_detectionDistance, destructibleLayer);
        float distance = Vector2.Distance(transform.position, targetTransform.position);
        bool hasSight = CheckSight(targetTransform.position, worldLayer);
        bool isInAngle = CheckAngle(transform.position, transform.up, targetTransform.position, _detectionAngle);
        //Debug.Log(distance + " " + hasSight + " " + isInAngle);
        if (hasSight && isInAngle && distance <= _detectionDistance && targetTransform.gameObject.activeSelf)
        {
            RotateTurretTowards(targetTransform.position);
            _targetLossTime = _targetLossDelay;
            if (_fireRecoveryDelay <= 0f)
                Shoot();
        }
        else if (_targetLossTime <= 0f)
        {
            RotateTurretTowards(transform.position + transform.up * 10);
        }
        _targetLossTime -= Time.deltaTime;
        _fireRecoveryDelay -= Time.deltaTime;
    }

    private void Shoot()
    {


        if (_fireRecoveryDelay <= 0f)
        {
            GameObject attackPrefab = _attackPrefabPool.Dequeue();
            attackPrefab.transform.position = MuzzleTransform.position + MuzzleTransform.up;
            attackPrefab.SetActive(true);
            attackPrefab.transform.rotation = MuzzleTransform.rotation;

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
                Debug.LogWarning(_shootGOPool.tag + " does not contain a ProjectileScript script");
            }
            _attackPrefabPool.Enqueue(attackPrefab);
            _fireRecoveryDelay = fireCooldown;
        }
    }

    /// <summary>
    /// Finds out if the angle is less than the given angle
    /// </summary>
    /// <param name="position">position of the detector</param>
    /// <param name="upVector">Vector aiming at the desired angle</param>
    /// <param name="target"> target postion</param>
    /// <param name="maxAngle"> Returns true if angle isbelow this value</param>
    /// <returns></returns>
    public bool CheckAngle(Vector2 position,Vector2 upVector, Vector2 target, float maxAngle)
    {
        if (Vector2.Angle(upVector, target - position) <= maxAngle)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if there is nothing of the given layermaks in to the target from transform
    /// </summary>
    /// <param name="target">the vector 3 is converted to a vector 2</param>
    /// <param name="layermask">what will be taken into account when cheking for line of sight</param>
    /// <returns></returns>
    protected bool CheckSight(Vector3 target, LayerMask layermask)
    {
        Vector2 start = new Vector2(transform.position.x, transform.position.y);
        Vector2 end = new Vector2(target.x, target.y);
        float thisDistance = Vector2.Distance(start, end);
        end = new Vector2(target.x - start.x, target.y - start.y);
        RaycastHit2D CheckHit = Physics2D.Raycast(start, end, thisDistance, layermask);
        if (CheckHit == false)
        {
            //Debug.DrawLine(start, end, Color.green);
            return true;
        }
        //Debug.Log(CheckHit.transform.name);

        return false;
    }

    private void RotateTurretTowards(Vector3 target)
    {
        Vector2 relativeTargetPos = (target - _gunTranform.position);
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, relativeTargetPos);

        _gunTranform.rotation = Quaternion.RotateTowards(_gunTranform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
    }
    public void FindEnemyInArea(float range, LayerMask enemyTeamsLayer)
    {
        Collider2D[] foundTargets = Physics2D.OverlapCircleAll(transform.position, range, enemyTeamsLayer);

        HashSet<Transform> targets = new HashSet<Transform>();

        foreach (Collider2D foundTarget in foundTargets)
        {
            if (CheckSight(foundTarget.transform.position, worldLayer) && 
                CheckAngle(transform.position, transform.up, foundTarget.transform.position, _detectionAngle))
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
        Array.Sort(transforms, new DistanceComparer(transform));
        if (transforms.Length != 0)
        {
            targetTransform = transforms[0];
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionDistance);
    }

}
