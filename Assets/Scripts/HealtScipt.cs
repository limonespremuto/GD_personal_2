using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtScipt : MonoBehaviour, IHealth
{
    [SerializeField]
    ScritableObjectHealth cHealthStats;

    [SerializeField]
    float currentHealth;


    private void Start()
    {
        currentHealth = cHealthStats.maxHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage * (1f - cHealthStats.resistance);
        if (currentHealth <= 0f)
        {
            DisableGO();
        }
    }
    public void DisableGO()
    {
        gameObject.SetActive(false);
    }
}

public interface IHealth
{
    public void TakeDamage(float damage);

}