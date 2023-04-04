using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOPoolScript : MonoBehaviour
{
    public static GOPoolScript instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        instance = this;
        foreach (Pool pool in pools)
        {
            //Debug.Log(pool.tag + " " + pool.prefab.name + " " + pool.size);
            CreatePool(pool.tag, pool.prefab, pool.size);
        }

    }

    private void Start()
    {
        //initializing common pools.
    }
    public void CreatePool(string tag, GameObject prefab, int size)
    {
        //if (!poolDictionary.ContainsKey(tag))
        //{
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

        
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(tag, objectPool);
        //}

    }
    
    public void ExtendPool(string tag, GameObject prefab, int howManyToAdd)
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        if (poolDictionary.TryGetValue(tag, out Queue<GameObject> pool))
        {
            for (int i = 0; i < howManyToAdd; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }
    }
}
