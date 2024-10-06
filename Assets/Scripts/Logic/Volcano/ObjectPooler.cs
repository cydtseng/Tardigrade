using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public GameObject objectToPool;
    public int poolSize = 20;
    private List<GameObject> pooledObjects;

    void Start()
    {
        if (objectToPool == null)
        {
            Debug.LogError("ObjectPooler: No object assigned to pool. Please assign a prefab in the inspector.");
            return;
        }

        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            if (obj != null)
            {
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
            else
            {
                Debug.LogWarning("ObjectPooler: Instantiated object is null.");
            }
        }
    }

    public GameObject GetPooledObject()
    {
        if (pooledObjects == null)
        {
            Debug.LogError("ObjectPooler: Pooled objects list is null. Ensure that Start was called properly.");
            return null;
        }
        
        foreach (var obj in pooledObjects)
        {
            if (obj != null && !obj.activeInHierarchy)
            {
                return obj;
            }
        }
        
        if (objectToPool != null)
        {
            GameObject newObj = Instantiate(objectToPool);
            if (newObj != null)
            {
                newObj.SetActive(false);
                pooledObjects.Add(newObj);
                return newObj;
            }
            else
            {
                Debug.LogWarning("ObjectPooler: Failed to instantiate new object.");
            }
        }
        else
        {
            Debug.LogError("ObjectPooler: objectToPool is null. Cannot instantiate new objects.");
        }

        return null;
    }
}
