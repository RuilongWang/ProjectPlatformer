using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPool <T> : MonoBehaviour where T : MonoBehaviour  {
    public T[] listOfObjectToPool;
    public int initialAmountToPoolPerObject = 10;

    private Queue<T> queuedObjectsInPool = new Queue<T>();

    #region Monobehaviour methods
    private void Awake()
    {

    }


    
    #endregion monobehaviour methods

    public void Spawn(T prefabToSpawn)
    {

    }

    public void Despawn(T objectToDespawn)
    {

    }

    protected virtual T CreateNewObjectForSpawnPool(T prefabToPool)
    {
        return Instantiate<T>(prefabToPool);
    }
}
