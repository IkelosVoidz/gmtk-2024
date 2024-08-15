using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum PoolType
{
    Particles,
    GameObject,
    AudioSources,
    None,
}

public class ObjectPoolingManager : StaticSingleton<ObjectPoolingManager>
{
    public List<ObjectPool> ObjectPools = new List<ObjectPool>();

    private GameObject _objectPoolEmptyHolder;
    private GameObject _particleEmpty;
    private GameObject _gameObjectEmpty;
    private GameObject _audioSourceEmpty;

    private void Awake()
    {
        base.Awake();
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");
        _objectPoolEmptyHolder.transform.SetParent(this.transform);
        _particleEmpty = new GameObject("Particle Effects");
        _particleEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
        _audioSourceEmpty = new GameObject("Audio Sources");
        _audioSourceEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
        _gameObjectEmpty = new GameObject("GameObjects");
        _gameObjectEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }

    public GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType type = PoolType.None)
    {
        ObjectPool pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new ObjectPool() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.AvaliableObjects.FirstOrDefault();
        if (spawnableObj == null)
        {
            GameObject parent = SetParentObject(type);
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parent != null)
            {
                spawnableObj.transform.SetParent(parent.transform);
            }
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.AvaliableObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {

        string realName = obj.name.Substring(0, obj.name.Length - 7);
        ObjectPool pool = ObjectPools.Find(p => p.LookupString == realName);
        if (pool == null)
        {
            Debug.LogWarning("Tying to release an object that hasnt been pooled" + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.AvaliableObjects.Add(obj);
        }
    }

    public void ReturnObjectToPool(GameObject obj, float delay)
    {
        string realName = obj.name.Substring(0, obj.name.Length - 7);
        ObjectPool pool = ObjectPools.Find(p => p.LookupString == realName);
        if (pool == null)
        {
            Debug.LogWarning("Tying to release an object that hasnt been pooled" + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.AvaliableObjects.Add(obj);
        }
    }

    private GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.Particles:
                return _particleEmpty;
            case PoolType.GameObject:
                return _gameObjectEmpty;
            case PoolType.AudioSources:
                return _audioSourceEmpty;
            case PoolType.None:
                return null;
            default:
                return null;
        }
    }
}

public class ObjectPool
{
    public string LookupString;
    public List<GameObject> AvaliableObjects = new List<GameObject>();
}
