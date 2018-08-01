using System.Collections.Generic;
using UnityEngine;

public class SpawnPool : MonoBehaviour  {
    #region static variables
    private static SpawnPool instance;

    public static SpawnPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SpawnPool>();
            }

            return instance;
        }
    }
    #endregion static variables

    public PrefabStruct[] listOfObjectToPool;
    private Transform parentToStorePooledObjects;

    private Dictionary<string, Queue<MonoBehaviour>> dictionayOfPooledObjects = new Dictionary<string, Queue<MonoBehaviour>>();

    #region Monobehaviour methods
    private void Awake()
    {
        instance = this;
        if (!parentToStorePooledObjects)
        {
            parentToStorePooledObjects = this.transform;
        }
    }

    private void Start()
    {
        
        InitializeSpawnPool();
    }
    
    #endregion monobehaviour methods

    /// <summary>
    /// Dequeues a previously inactive gameobject that is an instantiation of the prefab passed it
    /// and returns it as active
    /// </summary>
    /// <param name="prefabToSpawn"></param>
    /// <returns></returns>
    public virtual MonoBehaviour Spawn(MonoBehaviour prefabToSpawn)
    {
        if (!dictionayOfPooledObjects.ContainsKey(prefabToSpawn.name))
        {
            Debug.Log("This spawnpool does not contain a reference for the object '" + prefabToSpawn.name + "'");
            return null;
        }

        Queue<MonoBehaviour> queuedObjectsInPool = dictionayOfPooledObjects[prefabToSpawn.name];
        if (queuedObjectsInPool.Count == 0)
        {
            CreateNewObjectForSpawnPool(prefabToSpawn);
        }

        MonoBehaviour objectThatIsBeingSpawned = queuedObjectsInPool.Dequeue();
        objectThatIsBeingSpawned.gameObject.SetActive(true);

        return objectThatIsBeingSpawned;
    }

    /// <summary>
    /// Despawns an object and add it back to its respective queue to be used
    /// when it is needed
    /// </summary>
    /// <param name="objectToDespawn"></param>
    public virtual void Despawn(MonoBehaviour objectToDespawn)
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

    /// <summary>
    /// Intiailzes the spawn pool with the necessary game objects
    /// </summary>
    protected virtual void InitializeSpawnPool()
    {
        foreach (PrefabStruct prefabStruct in listOfObjectToPool)
        {
            for (int i = 0; i < prefabStruct.initialNumberOfSpawnedObjects; i++)
            {
                CreateNewObjectForSpawnPool(prefabStruct.prefabObject);
            }
        }
    }

    /// <summary>
    /// Instantiates a new object into the world if there are not enough currently stored
    /// in the spawn pool queue
    /// </summary>
    /// <param name="prefabToPool"></param>
    protected virtual void CreateNewObjectForSpawnPool(MonoBehaviour prefabToPool)
    {
        MonoBehaviour newObjectToAddToPool = Instantiate<MonoBehaviour>(prefabToPool);
        newObjectToAddToPool.gameObject.SetActive(false);
        if (!dictionayOfPooledObjects.ContainsKey(prefabToPool.name))
        {
            dictionayOfPooledObjects.Add(prefabToPool.name, new Queue<MonoBehaviour>());
        }
        newObjectToAddToPool.transform.SetParent(parentToStorePooledObjects);
        string keyword = "(Clone)";
        newObjectToAddToPool.name = newObjectToAddToPool.name.Substring(0, newObjectToAddToPool.name.Length - keyword.Length);
        dictionayOfPooledObjects[prefabToPool.name].Enqueue(newObjectToAddToPool);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddObjectsToSpawnPoolIfNotAddedAlready(MonoBehaviour prefabToPool, int numberToAdd = 10)
    {
        string prefabName = prefabToPool.name;
        int adjustedNumberToAdd = numberToAdd;
        if (dictionayOfPooledObjects.ContainsKey(prefabName))
        {
            adjustedNumberToAdd = dictionayOfPooledObjects[prefabName].Count;
        }
        
        for (int i = 0; i < adjustedNumberToAdd; i++)
        {
            CreateNewObjectForSpawnPool(prefabToPool);
        }
    }

    [System.Serializable]
    public struct PrefabStruct
    {
        public MonoBehaviour prefabObject;
        public int initialNumberOfSpawnedObjects;
    }
}
