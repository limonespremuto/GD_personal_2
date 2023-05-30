using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float activeTime;
    private float remainingTime;

    public float damage;
    public float speed;
    public LayerMask layerCheck;
    public AIBase.ETeam myTeam;

    // Start is called before the first frame update
    void OnEnable()
    {
        remainingTime = activeTime;
    }

    // Update is called once per frame
    void Update()
    {
        remainingTime -= Time.deltaTime;
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(transform.position, transform.up, speed * Time.deltaTime, layerCheck))
        {
            IHealth ihealt = hit.transform.GetComponent<IHealth>();
            AIBase aiBase = hit.transform.GetComponent<AIBase>();
            if (ihealt != null)
            {
                if (aiBase != null)
                {
                    if (aiBase.myTeam != myTeam)
                    {
                        ihealt.TakeDamage(damage);
                    }
                }
                else
                {
                    ihealt.TakeDamage(damage);
                }
            }

            if (aiBase != null)
            {
                if (aiBase.myTeam != myTeam)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }


        }
        transform.position += transform.up * speed * Time.deltaTime;
        if (remainingTime <= 0f)
            gameObject.SetActive(false);

    }
}
