using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPooling : MonoBehaviour
{
    public List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] private int poolingAmount;
    public GameObject[] prefabs;

    private void Start()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            for (int j = 0; j < poolingAmount; j++)
            {
                GameObject obj = Instantiate(prefabs[i]);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetObject()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

}
