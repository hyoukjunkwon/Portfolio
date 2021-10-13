using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectPool
{
    public GameObject prefab;
    public int size;
}

public class Pool: MonoBehaviour
{
    public List<ObjectPool> ObjectPoolList;

    private Dictionary<string, Queue<GameObject>> objectPoolDictionary;

    #region SingleTone
    private static Pool _instance = null;

    public static Pool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (Pool)FindObjectOfType(typeof(Pool));
                if (_instance == null)
                {
                    Debug.Log("There's no active Singletone object");
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            _instance = this;
            Init();
        }
    }
    #endregion

    private void Init()
    {
        objectPoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (ObjectPool pool in ObjectPoolList)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = GameObject.Instantiate(pool.prefab);
                obj.name = pool.prefab.name;
                obj.SetActive(false);
                obj.transform.parent = this.transform;
                poolQueue.Enqueue(obj);
            }
            objectPoolDictionary.Add(pool.prefab.name, poolQueue);
        }
    }

    public GameObject Spawn(string poolName, Vector3 position, Quaternion rotation)
    {
        GameObject obj = null;
        if (objectPoolDictionary.ContainsKey(poolName))
        {
            if (objectPoolDictionary[poolName].Count > 0)
            {
                obj = objectPoolDictionary[poolName].Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
            }
            else
            {
                Debug.Log(poolName + " is empty");
            }
        }
        else
        {
            Debug.Log(poolName + " object pool is not available");
        }
        return obj;
    }

    private IEnumerator _despawn(GameObject poolObject, float timer)
    {
        yield return new WaitForSeconds(timer);
        if (objectPoolDictionary.ContainsKey(poolObject.name))
        {
            objectPoolDictionary[poolObject.name].Enqueue(poolObject);
            poolObject.SetActive(false);
        }
        else
        {
            Debug.Log(poolObject.name + " object pool is not available");
        }
    }

    public void Despawn(GameObject poolObject, float timer = 0f)
    {
        StartCoroutine(_despawn(poolObject, timer));
    }
}

