using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public abstract class GenericSpawner<Type> : MonoBehaviour where Type : SpawnableObject
{
    [SerializeField] private Type _prefab;
    [SerializeField] private SpawnZone _spawnZone;
    [SerializeField] private int _poolDefaultCapacity = 20;
    [SerializeField] private int _poolMaxSize = 100;
    [SerializeField] private List<Type> _spawnedObjects;
    [SerializeField] private Material _errorMaterial;

    private ObjectPool<Type> _pool;

    private void Awake()
    {
        if (_prefab == null || _spawnZone == null)
            throw new System.ArgumentNullException();

        InitializePool();
    }

    public Type Spawn()
    {
        return _pool.Get();
    }

    private void InitializePool()
    {
        _pool = new ObjectPool<Type>(
            createFunc: () => Create(),
            actionOnGet: (spawnedObject) => PrepareForSpawn(spawnedObject),
            actionOnRelease: (spawnedObject) => spawnedObject.gameObject.SetActive(false),
            actionOnDestroy: (spawnedObject) => Destroy(spawnedObject.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize
            );
    }

    private Type PrepareForSpawn(Type spawnedObject)
    {
        spawnedObject.transform.position = _spawnZone.GetRandomSpawnPosition();
        spawnedObject.PrepareForSpawn();
        spawnedObject.gameObject.SetActive(true);

        return spawnedObject;
    }

    public void ReturnToPool(Type spawnedObject)
    {
        PrepareToDeactivate(spawnedObject);

        try
        {
            _pool.Release(spawnedObject);
        }
        catch (InvalidOperationException ex)
        {
            Debug.Log(ex.Message + " name = " + spawnedObject.gameObject.name);
            spawnedObject.transform.localScale *= 1.5f;
        }
    }

    public virtual void PrepareToDeactivate(Type spawnedObject) { }


    private Type Create()
    {
        Type spawnedObject = Instantiate(_prefab);
        _spawnedObjects.Add(spawnedObject);

        return spawnedObject;
    }
}