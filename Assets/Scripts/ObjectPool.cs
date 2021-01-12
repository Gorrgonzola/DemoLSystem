using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject[] variants;
    public string type;
    public int amountToPool;
    public bool shouldExpand;
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private List<ObjectPoolItem> itemsToPool = null;

    private readonly Dictionary<Type, List<GameObject>> pooledObjects = new Dictionary<Type, List<GameObject>>();

    void Awake()
    {
        foreach (ObjectPoolItem item in itemsToPool)
        {
            pooledObjects.Add(Type.GetType(item.type), new List<GameObject>());
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            var t = Type.GetType(item.type);
            for (int i = 0; i < item.amountToPool; i++)
            {
                var ri = UnityEngine.Random.Range(0, item.variants.Length);
                var obj = Instantiate(item.variants[ri], gameObject.transform);
                obj.SetActive(false);
                pooledObjects[t].Add(obj);
            }
        }
    }
    public GameObject GetPooledObject(Type objectType)
    {
        var objectTypesInPool = pooledObjects[objectType];

        for (int i = 0; i < objectTypesInPool.Count; i++)
        {
            if (!objectTypesInPool[i].activeInHierarchy)
            {
                return objectTypesInPool[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (Type.GetType(item.type) == objectType)
            {
                if (item.shouldExpand)
                {
                    var ri = UnityEngine.Random.Range(0, item.variants.Length - 1);
                    var obj = Instantiate(item.variants[ri], gameObject.transform);
                    obj.SetActive(false);
                    pooledObjects[Type.GetType(item.type)].Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
    public void CleanUp()
    {
        foreach (var listObjs in pooledObjects)
        {
            foreach (var obj in listObjs.Value)
            {
                if (obj.activeInHierarchy)
                {
                    for(int i = 0; i < obj.transform.childCount;i++)
                    {
                        var ch = obj.transform.GetChild(i).gameObject;
                        if (ch.name.Equals("Floor"))
                            continue;
                        ch.SetActive(false);
                    }
                    obj.SetActive(false);
                }
            }
        }

    }
}
