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
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        //initializing common pools.
        foreach (Pool pool in pools)
        {
            //Debug.Log(pool.tag + " " + pool.prefab.name + " " + pool.size);
            CreatePool(pool.tag, pool.prefab, pool.size);
        }
    }

    private void Start()
    {

    }
    public void CreatePool(string tag, GameObject prefab, int size)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        //Debug.Log(objectPool.Count);

        poolDictionary.Add(tag, objectPool);

    }

    public void ExtendPools(Pool newPool)
    {
        if (poolDictionary.ContainsKey(newPool.tag) == false)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < newPool.size ; i++)
            {
                GameObject obj = Instantiate(newPool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(newPool.tag, objectPool);
        }
    }
}