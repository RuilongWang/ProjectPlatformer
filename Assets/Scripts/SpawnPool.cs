using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnPool <T> : MonoBehaviour where T : MonoBehaviour  {
    public T[] listOfObjectToPool;
    public int initialAmountToPoolPerObject = 10;
    public Transform parentToStorePooledObjects;

    private Dictionary<string, Queue<T>> dictionayOfPooledObjects = new Dictionary<string, Queue<T>>();

    #region Monobehaviour methods
    private void Start()
    {
        if (!parentToStorePooledObjects)
        {
            parentToStorePooledObjects = this.transform;
        }
        InitializeSpawnPool();
    }
    
    #endregion monobehaviour methods

    public virtual T Spawn(T prefabToSpawn)
    {
        if (!dictionayOfPooledObjects.ContainsKey(prefabToSpawn.name))
        {
            Debug.Log("This spawnpool does not contain a reference for the object '" + prefabToSpawn.name + "'");
            return null;
        }

        Queue<T> queuedObjectsInPool = dictionayOfPooledObjects[prefabToSpawn.name];
        if (queuedObjectsInPool.Count == 0)
        {
            CreateNewObjectForSpawnPool(prefabToSpawn);
        }

        T objectThatIsBeingSpawned = queuedObjectsInPool.Dequeue();
        objectThatIsBeingSpawned.gameObject.SetActive(true);
        SetupObjectWhenSpawned(objectThatIsBeingSpawned);

        return objectThatIsBeingSpawned;
    }

    public virtual void Despawn(T objectToDespawn)
    {
        if (!dictionayOfPooledObjects.ContainsKey(objectToDespawn.name))
        {
            Debug.Log("This spawnpool does not contain a reference for the object '" + objectToDespawn.name + "'");
            return;
        }
        objectToDespawn.gameObject.SetActive(false);
        objectToDespawn.transform.SetParent(parentToStorePooledObjects);
        dictionayOfPooledObjects[objectToDespawn.name].Enqueue(objectToDespawn);

    }

    protected virtual void InitializeSpawnPool()
    {
        foreach (T prefabToCreate in listOfObjectToPool)
        {
            for (int i = 0; i < initialAmountToPoolPerObject; i++)
            {
                CreateNewObjectForSpawnPool(prefabToCreate);
            }
        }
        
    }

    protected virtual void CreateNewObjectForSpawnPool(T prefabToPool)
    {
        T newObjectToAddToPool = Instantiate<T>(prefabToPool);
        newObjectToAddToPool.gameObject.SetActive(false);
        if (!dictionayOfPooledObjects.ContainsKey(prefabToPool.name))
        {
            dictionayOfPooledObjects.Add(prefabToPool.name, new Queue<T>());
        }
        newObjectToAddToPool.transform.SetParent(parentToStorePooledObjects);
        dictionayOfPooledObjects[prefabToPool.name].Enqueue(newObjectToAddToPool);
    }

    protected virtual void SetupObjectWhenSpawned(T objectToSetup)
    {

    }
}
