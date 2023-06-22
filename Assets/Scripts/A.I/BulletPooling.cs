using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : MonoBehaviour
{
    public List<GameObject> pooledObject = new List<GameObject>();
    [SerializeField] private int PoolingAmount;
    [SerializeField] private GameObject prefab;
    private bool canExpand;

    private void Start()
    {
        for(int i = 0; i < PoolingAmount; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pooledObject.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        for(int i = 0; i < pooledObject.Count; i++)
        {
            if (!pooledObject[i].activeInHierarchy)
            {
                return pooledObject[i];
            }
        }

        if (canExpand)
        {
            GameObject obj = Instantiate(prefab);
            pooledObject.Add(obj);
            return obj;
        }
        return null;
    }
}
