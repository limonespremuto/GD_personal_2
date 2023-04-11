using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanLineShootVFX : MonoBehaviour
{


    [HideInInspector]
    public float lifeTime;
    public float MaxLifeTime;

    public LineRenderer lineEffect;
    public Gradient ColorOverLifetime;
    
    [Header("HitScan type")]
    [Tooltip("how many points the line will have over distance, if 0 only begining and end")]
    public float lineDensitiy;

    [HideInInspector]
    public Vector3 endPos;
    public Transform endEffectTransform;

    public ProjectileType projectileType = ProjectileType.hitscan;
    public enum ProjectileType
    {
        hitscan
    }

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
            endEffectTransform.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        switch (projectileType)
        {
            case ProjectileType.hitscan:
            {

                break;
            }
        }


        if (lineEffect != null)
        {
            lineEffect.startColor = ColorOverLifetime.Evaluate(1f - lifeTime / MaxLifeTime);
            lineEffect.endColor = ColorOverLifetime.Evaluate(1f - lifeTime / MaxLifeTime);
        }
    }

    /// <summary>
    /// Initializes the effects of having shot a hitscan weapon
    /// </summary>
    /// <param name="startPos"> the position where the line begins</param>
    /// <param name="hitPos"> the position where the line ends</param>
    /// <param name="hitNormal"> the normal of the hit, to rotate the hit effect</param>
    public void InitializeProjcetileHitscan(Vector3 startPos, Vector3 hitPos, Vector3 hitNormal)
    {
        int NumberOflineIntersection;
        NumberOflineIntersection = Mathf.RoundToInt(Vector3.Distance(startPos,hitPos) / lineDensitiy);
        if (NumberOflineIntersection <= 1 || lineDensitiy == 0)
        {
            lineEffect.positionCount = 2;
            lineEffect.SetPosition(0, startPos);
            lineEffect.SetPosition(1, hitPos);
        }
        else
        {
            //Debug.Log(NumberOflineIntersection);
            
            lineEffect.positionCount = NumberOflineIntersection;
            for (int i = 0; i < NumberOflineIntersection; i++)
            {
                lineEffect.SetPosition(i, Vector3.Lerp(startPos,hitPos,(float)i / NumberOflineIntersection));
            }
            lineEffect.SetPosition(NumberOflineIntersection - 1, hitPos);
        }

        lifeTime = MaxLifeTime;
        endEffectTransform.position = hitPos;
        endEffectTransform.up = hitNormal;
        endEffectTransform.gameObject.SetActive(true);
    }
}
