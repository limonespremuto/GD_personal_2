using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime;
    public float MaxLifeTime;
    public LineRenderer lineEffect;
    public Gradient ColorOverLifetime;

    [HideInInspector]
    public Vector3 endPos;
    public Transform endEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            endEffect.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        if (lineEffect != null)
        {
            lineEffect.startColor = ColorOverLifetime.Evaluate(1f - lifeTime / MaxLifeTime);
            lineEffect.endColor = ColorOverLifetime.Evaluate(1f - lifeTime / MaxLifeTime);
        }
    }

    public void InitializeProjcetile(Vector3 startPos, Vector3 hitPos, Vector3 hitNormal)
    {
        lifeTime = MaxLifeTime;
        lineEffect.SetPosition(0, startPos);
        lineEffect.SetPosition(1, hitPos);
        endEffect.position = hitPos;
        endEffect.up = hitNormal;
        endEffect.gameObject.SetActive(true);
    }
}
