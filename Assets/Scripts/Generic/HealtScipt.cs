using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtScipt : MonoBehaviour, IHealth
{
    [SerializeField]
    ScritableObjectHealth cHealthStats;

    [SerializeField]
    float currentHealth;
    float maxHealth;
    float resistance;

    [SerializeField]
    private GameObject[] ItemsDroppedOnDeath;

    private void Start()
    {
        currentHealth = cHealthStats.maxHealth;
        maxHealth = cHealthStats.maxHealth;
        resistance = cHealthStats.resistance;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage * (1f - resistance);
        if (currentHealth <= 0f)
        {
            foreach (GameObject gO in ItemsDroppedOnDeath)
            {
                Vector3 droppedItemPos = transform.position;
                droppedItemPos.y += Random.Range(0f, 0.2f);
                droppedItemPos.x += Random.Range(0f, 0.2f);
                Instantiate(gO, droppedItemPos, Quaternion.identity);
            }
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